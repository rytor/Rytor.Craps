using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Interfaces
{
    public interface IBalanceService
    {
        Task<Balance> GetBalance(int accountId);
    }
}