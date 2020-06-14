using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    public interface IGameService
    {
        Models.Game ResetGame();
        Models.Game HandleRoll(RollResult roll, Models.Game game);
    }
}