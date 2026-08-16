using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.Api.Services
{
    public class GameManager
    {
        public Dictionary<Guid, Game> Games { get; }
            = new();

        public Dictionary<string, string> PlayerConnections { get; }
            = new();
    }
}