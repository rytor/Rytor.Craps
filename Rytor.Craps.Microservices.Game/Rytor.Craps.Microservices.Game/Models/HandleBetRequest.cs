using System.Collections.Generic;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Models
{
    public class HandleBetRequest
    {
        public Models.Game Game { get; set; }
        public RollResult Roll { get; set; }
        public IEnumerable<Bet> Bets { get; set; }
    }
}