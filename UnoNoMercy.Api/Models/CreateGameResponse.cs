using UnoNoMercy.GameEngine.DTOs;

namespace UnoNoMercy.Api.Models
{
    public class CreateGameResponse
    {
        public Guid GameId { get; set; }

        public GameStateDto State { get; set; }
    }
}
