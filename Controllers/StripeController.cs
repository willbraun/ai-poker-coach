using ai_poker_coach.Models.Domain;
using DotNet8Authentication.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Stripe;

namespace ai_poker_coach.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly string _stripeApiKey;
        private readonly string _stripeWebhookSecret;
        private readonly IdentityDataContext _dbContext;

        public StripeController(IdentityDataContext dbContext)
        {
            _dbContext = dbContext;
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");

            if (environment == "Development")
            {
                _stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY_TEST") ?? "";
                _stripeWebhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_TEST") ?? "";
            }
            else
            {
                _stripeApiKey = Environment.GetEnvironmentVariable("STRIPE_API_KEY_PRODUCTION") ?? "";
                _stripeWebhookSecret = Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_PRODUCTION") ?? "";
            }

            if (string.IsNullOrEmpty(_stripeApiKey))
            {
                throw new Exception("Stripe API key is not set in: " + environment);
            }

            if (string.IsNullOrEmpty(_stripeWebhookSecret))
            {
                throw new Exception("Stripe webhook secret is not set in: " + environment);
            }

            StripeConfiguration.ApiKey = _stripeApiKey;
        }

        [HttpPost]
        public async Task<IActionResult> HandleWebhook()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                var stripeEvent = EventUtility.ConstructEvent(
                    json,
                    Request.Headers["Stripe-Signature"],
                    _stripeWebhookSecret
                );

                switch (stripeEvent.Type)
                {
                    case Events.CustomerSubscriptionCreated:
                        if (stripeEvent.Data.Object is Subscription newSubscription)
                        {
                            var user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u =>
                                u.StripeCustomerId == newSubscription.CustomerId
                            );

                            if (user == null)
                            {
                                // If user is not found by StripeCustomerId, try to find by email
                                var customer = await new CustomerService().GetAsync(newSubscription.CustomerId);
                                user = await _dbContext.ApplicationUsers.FirstOrDefaultAsync(u =>
                                    u.Email == customer.Email
                                );

                                if (user != null)
                                {
                                    // Update the user with the Stripe Customer ID
                                    user.StripeCustomerId = newSubscription.CustomerId;
                                    await _dbContext.SaveChangesAsync();
                                }
                            }

                            if (user != null)
                            {
                                var stripeSubscription = new StripeSubscription(user, newSubscription);
                                await _dbContext.StripeSubscriptions.AddAsync(stripeSubscription);
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                throw new Exception(
                                    $"Create StripeSubscription failed: User not found for Stripe Customer ID: {newSubscription.CustomerId}"
                                );
                            }
                        }
                        break;

                    case Events.CustomerSubscriptionUpdated:
                        if (stripeEvent.Data.Object is Subscription updatedSubscription)
                        {
                            Console.WriteLine("updatedSubscription: " + updatedSubscription);
                            var stripeSubscription = await _dbContext.StripeSubscriptions.FirstOrDefaultAsync(s =>
                                s.SubscriptionId == updatedSubscription.Id
                            );
                            if (stripeSubscription != null)
                            {
                                stripeSubscription.Status = updatedSubscription.Status;
                                stripeSubscription.PriceId = updatedSubscription.Items.Data[0].Price.Id;
                                stripeSubscription.StartDate = updatedSubscription.StartDate;
                                stripeSubscription.EndDate = updatedSubscription.EndedAt;
                                await _dbContext.SaveChangesAsync();
                            }
                            else
                            {
                                throw new Exception(
                                    $"Update StripeSubscription failed: StripeSubscription not found for Subscription ID: {updatedSubscription.Id}"
                                );
                            }
                        }
                        break;

                    case Events.CustomerSubscriptionDeleted:
                        var deletedSubscription = stripeEvent.Data.Object as Subscription;
                        Console.WriteLine("deletedSubscription: " + deletedSubscription);
                        // Handle subscription deletion logic
                        break;

                    default:
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                Console.WriteLine("StripeException: " + e.Message);
                return BadRequest(e.Message);
            }
        }
    }
}
