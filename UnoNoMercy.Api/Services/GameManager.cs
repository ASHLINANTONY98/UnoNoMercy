using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.Api.Services
{
    public class GameManager
    {
        public Dictionary<Guid, Game> Games { get; }
            = new();

        public Dictionary<string, string> PlayerConnections { get; }
            = new();

        public Dictionary<string, PlayerSession> PlayerSessions { get; }
            = new();

        public bool TryGetSession(
            string sessionToken,
            out PlayerSession session)
        {
            return PlayerSessions.TryGetValue(
                sessionToken,
                out session!);
        }

        public void RemoveExpiredSessions(
            TimeSpan expiration)
        {
            var now = DateTime.UtcNow;

            var expiredTokens =
                PlayerSessions
                    .Where(x =>
                        now - x.Value.LastActivityUtc
                        > expiration)
                    .Select(x => x.Key)
                    .ToList();

            foreach (var token in expiredTokens)
            {
                PlayerSessions.Remove(token);
            }
        }
    }
}