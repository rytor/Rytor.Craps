namespace Rytor.Craps.Microservices.Orchestrator.Models
{
    public class Game
    {
        public GameState State { get; set; }
        public int NumberOfRolls { get; set; }
        public List<GameEvent> LastGameEvents { get; set; }
        public int Point { get; set; }
        public bool Completed { get; set; }
    }
}