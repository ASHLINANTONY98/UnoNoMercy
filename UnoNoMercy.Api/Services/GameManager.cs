using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.Api.Services
{
    public class GameManager
    {
        public Dictionary<Guid, Game> Games { get; }
            = new();
    }
}