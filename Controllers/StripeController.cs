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
        private readonly string _stripeWebhookSecret;

        private readonly IdentityDataContext _dbContext;

        public StripeController(IdentityDataContext dbContext)
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _stripeWebhookSecret =
                environment == "Development"
                    ? Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_LOCAL")
                        ?? "Could not load local Stripe webhook secret"
                    : Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_PRODUCTION")
                        ?? "Could not load production Stripe webhook secret";
            _dbContext = dbContext;
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
                                Console.WriteLine($"Added new StripeSubscription: {stripeSubscription.Id}");
                            }
                            else
                            {
                                Console.WriteLine(
                                    $"User not found for Stripe Customer ID: {newSubscription.CustomerId}"
                                );
                            }
                        }
                        break;

                    case Events.CustomerSubscriptionUpdated:
                        var updatedSubscription = stripeEvent.Data.Object as Subscription;
                        Console.WriteLine("updatedSubscription: " + updatedSubscription);
                        // Handle subscription update logic
                        break;

                    case Events.CustomerSubscriptionDeleted:
                        var deletedSubscription = stripeEvent.Data.Object as Subscription;
                        Console.WriteLine("deletedSubscription: " + deletedSubscription);
                        // Handle subscription deletion logic
                        break;

                    default:
                        // Handle other event types
                        break;
                }

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }
    }
}
