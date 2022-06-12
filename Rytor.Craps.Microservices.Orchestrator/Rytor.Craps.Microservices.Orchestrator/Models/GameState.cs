namespace Rytor.Craps.Microservices.Orchestrator.Models
{
    public class GameState
    {
        public bool rolling { get; set; }
        public List<Bet> bets { get; set; }
        public string point { get; set; }
    }
}