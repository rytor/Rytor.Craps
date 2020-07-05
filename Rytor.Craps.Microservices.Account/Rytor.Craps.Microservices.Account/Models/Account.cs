using System;

namespace Rytor.Craps.Microservices.Account.Models
{
    public class Account
    {
        public int Id { get; set; }
        public string TwitchId { get; set; }
        public DateTime CreateDate { get; set; }
    }
}