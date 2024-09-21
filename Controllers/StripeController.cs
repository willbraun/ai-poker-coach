using Microsoft.AspNetCore.Mvc;
using Stripe;

namespace ai_poker_coach.Controllers
{
    [Route("webhook/[controller]")]
    [ApiController]
    public class StripeController : ControllerBase
    {
        private readonly string _stripeWebhookSecret;

        public StripeController()
        {
            var environment = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
            _stripeWebhookSecret =
                environment == "Development"
                    ? Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_LOCAL") ?? "Could not load local secret"
                    : Environment.GetEnvironmentVariable("STRIPE_WEBHOOK_SECRET_PRODUCTION")
                        ?? "Could not load production secret";
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
                        var newSubscription = stripeEvent.Data.Object as Subscription;
                        Console.WriteLine("newSubscription: " + newSubscription);
                        // Handle subscription creation logic
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
