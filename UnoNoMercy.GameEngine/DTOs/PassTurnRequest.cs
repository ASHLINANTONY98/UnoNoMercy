namespace UnoNoMercy.GameEngine.DTOs
{
    public class PassTurnRequest
    {
        public Guid GameId { get; set; }

        public string PlayerName { get; set; }
    }
}
