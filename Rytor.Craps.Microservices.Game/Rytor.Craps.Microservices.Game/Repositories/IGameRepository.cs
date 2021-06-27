using System.Collections.Generic;
using Rytor.Craps.Microservices.Game.Models;

namespace Rytor.Craps.Microservices.Game.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<GameEventPayout> GetGameEventPayouts();
        IEnumerable<Bet> GetBets();
        Bet GetBetById(int id);
        IEnumerable<Bet> GetBetsByStatus(BetStatus status);
        IEnumerable<Bet> GetBetsByAccountId(int accountId);
        int CreateBet(Bet bet);
        Bet UpdateBet(Bet bet);
        bool DeleteBet(int betId);
    }
}