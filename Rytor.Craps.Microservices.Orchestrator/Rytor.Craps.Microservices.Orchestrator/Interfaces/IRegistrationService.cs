using Rytor.Craps.Microservices.Orchestrator.Models;

namespace Rytor.Craps.Microservices.Orchestrator.Interfaces
{
    public interface IRegistrationService
    {
        Task<int> CheckInAccount(string twitchId);
    }
}