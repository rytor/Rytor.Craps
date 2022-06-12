namespace Rytor.Craps.Microservices.Orchestrator.Models
{
    public class BetRequest
    {
        public string TwitchId { get; set; }
        public int Amount { get; set; }
        public int GameEventType { get; set; }
    }
}