namespace Rytor.Craps.Microservices.Game.Models
{
    public class GameEventPayout
    {
        public GameEvent GameEventId { get; set; }
        public int PayoutOddsLeft { get; set; }
        public int PayoutOddsRight { get; set; }
    }
}