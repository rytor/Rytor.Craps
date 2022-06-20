using Rytor.Libraries.Dice;

namespace Rytor.Craps.Microservices.Orchestrator.Models
{
    public class GameUIState
    {
        public string Phase { get; set; }
        public int NumberOfRolls { get; set; }
        public string Point { get; set; }
        public int TimeLeft { get; set; }
        public RollResult Dice { get; set; }
        public List<BetState> bets { get; set; }
        public List<GameEvent> LastGameEvents { get; set; }
        public bool IsCompleted { get; set; }
    }
}