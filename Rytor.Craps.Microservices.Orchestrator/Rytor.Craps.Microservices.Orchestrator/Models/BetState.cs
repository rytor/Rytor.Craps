namespace Rytor.Craps.Microservices.Orchestrator.Models
{
    public class BetState
    {
        public string Player { get; set; }
        public int Amount { get; set; }
        public string Location { get; set; }
        public BetStatus Result { get; set; }
    }
}