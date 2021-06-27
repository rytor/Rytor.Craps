using System.Collections.Generic;
using Rytor.Craps.Microservices.Game.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    public interface IBetService
    {
        bool ValidateBet(Models.Game game, Bet bet);
        int CalculateBetPayout(Bet bet);
        IEnumerable<Bet> HandleBets(Models.Game game, RollResult roll, IEnumerable<Bet> bets);
    }
}