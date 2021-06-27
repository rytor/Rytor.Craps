using System;

namespace Rytor.Craps.Microservices.Game.Models
{
    public class Bet
    {
        public int Id { get; set; }
        public int AccountId { get; set; }
        public GameEvent GameEventId { get; set; }
        public int Amount { get; set; }
        public BetStatus BetStatusId { get; set; }
        public int Payout { get; set; }
        public DateTime CreateDate { get; set; }
    }
}