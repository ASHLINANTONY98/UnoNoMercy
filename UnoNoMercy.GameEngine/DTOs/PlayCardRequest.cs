namespace UnoNoMercy.GameEngine.DTOs
{
    public class PlayCardRequest
    {
        public Guid GameId { get; set; }

        public string PlayerName { get; set; }

        public string CardId { get; set; }
    }
}
