using System;

namespace Rytor.Craps.Microservices.Balance.Models
{
    public class Activity
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public ActivityType ActivityTypeId { get; set; }
        public int Amount { get; set; }
        public DateTime CreateDate { get; set; }
    }
}