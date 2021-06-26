using System.Collections.Generic;
using Rytor.Craps.Microservices.Game.Models;

namespace Rytor.Craps.Microservices.Game.Repositories
{
    public interface IGameRepository
    {
        IEnumerable<GameEventPayout> GetGameEventPayouts();
    }
}