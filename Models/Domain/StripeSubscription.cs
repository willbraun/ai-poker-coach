using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;
using Stripe;

namespace ai_poker_coach.Models.Domain
{
    public class StripeSubscription
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();
        public string SubscriptionId { get; set; } = "";
        public string CustomerId { get; set; } = "";
        public string PriceId { get; set; } = "";
        public string Status { get; set; } = "";
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }

        [ForeignKey("ApplicationUser")]
        public string ApplicationUserId { get; set; } = "";
        public ApplicationUser ApplicationUser { get; set; } = null!;

        public StripeSubscription() { }

        public StripeSubscription(ApplicationUser user, Subscription subscription)
        {
            ApplicationUserId = user.Id;
            ApplicationUser = user;
            SubscriptionId = subscription.Id;
            CustomerId = subscription.CustomerId;
            PriceId = subscription.Items.Data[0].Price.Id;
            Status = subscription.Status;
            StartDate = subscription.StartDate;
            EndDate = subscription.EndedAt;
        }
    }
}
