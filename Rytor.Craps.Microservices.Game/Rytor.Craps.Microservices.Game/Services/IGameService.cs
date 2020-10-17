using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Game.Services
{
    public interface IGameService
    {
        Models.Game HandleRoll(RollResult roll);
        Models.Game ResetGame();
        Models.Game AdvanceGameState();
        Models.Game OverrideGame(Models.Game newGame);
    }
}