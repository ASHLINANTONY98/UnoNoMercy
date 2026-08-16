namespace UnoNoMercy.Api.Services
{
    public class PlayerSession
    {
        public Guid GameId { get; set; }

        public string PlayerName { get; set; }
            = string.Empty;

        public string RoomCode { get; set; }
            = string.Empty;

        public string? ConnectionId { get; set; }
    }
}