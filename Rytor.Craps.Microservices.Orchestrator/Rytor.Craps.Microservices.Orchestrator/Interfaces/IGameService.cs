using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Interfaces
{
    public interface IGameService
    {
        Task<List<Bet>> GetBets();
    }
}