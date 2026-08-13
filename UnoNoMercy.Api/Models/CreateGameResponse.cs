using UnoNoMercy.GameEngine.DTOs;

namespace UnoNoMercy.Api.Models
{
    public class CreateGameResponse
    {
        public Guid GameId { get; set; }

        public string RoomCode { get; set; }
            = string.Empty;

        public GameStateDto State { get; set; }
    }

}
