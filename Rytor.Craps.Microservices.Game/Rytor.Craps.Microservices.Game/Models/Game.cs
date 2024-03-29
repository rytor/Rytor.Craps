using System.Collections.Generic;

namespace Rytor.Craps.Microservices.Game.Models
{
    public class Game
    {
        public GameState State { get; set; }
        public int NumberOfRolls { get; set; }
        public List<GameEvent> LastGameEvents { get; set; }
        public int Point { get; set; }
        public bool Completed { get; set; }

        public Game()
        {
            State = GameState.OpeningRollBets;
            NumberOfRolls = 0;
            LastGameEvents = new List<GameEvent>();
            Completed = false;
        }
    }
}