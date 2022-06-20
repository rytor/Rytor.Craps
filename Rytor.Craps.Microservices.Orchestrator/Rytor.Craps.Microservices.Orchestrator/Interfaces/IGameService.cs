using Rytor.Craps.Microservices.Orchestrator.Models;
using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Orchestrator.Interfaces
{
    public interface IGameService
    {
        Task<Game> GetGame();
        Task<List<Bet>> GetBets();
        RollResult GetLastDiceRoll();
        TimeSpan GetTimeLeftInInterval();
        bool StartAutomatedGame();
        bool EndAutomatedGame();
    }
}