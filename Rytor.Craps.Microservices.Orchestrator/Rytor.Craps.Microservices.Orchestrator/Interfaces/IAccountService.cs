using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Interfaces
{
    public interface IAccountService
    {
        Task<int> CheckInAccount(string twitchId);
        Task<List<Account>> GetAccounts();
    }
}