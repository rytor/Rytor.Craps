using System;

namespace Rytor.Craps.Microservices.Balance.Models
{
    public class Balance
    {
        public int AccountId { get; set; }
        public int CurrentBalance { get; set; }
        public int CurrentFloor { get; set; }
        public DateTime CreateDate { get; set; }
    }
}