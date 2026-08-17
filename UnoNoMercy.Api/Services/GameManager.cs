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
        public Dictionary<string, string> ActiveSessionConnections { get; }
            = new();

        public Dictionary<string, string> ActivePlayerConnections { get; }
            = new();

        public object SyncRoot { get; } = new();

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
            lock (SyncRoot)
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
                    if (PlayerSessions.TryGetValue(
                        token,
                        out var session))
                    {
                        if (ActiveSessionConnections.TryGetValue(
                            token,
                            out var connectionId))
                        {
                            ActiveSessionConnections.Remove(token);

                            if (PlayerConnections.TryGetValue(
                                connectionId,
                                out var playerName))
                            {
                                if (playerName ==
                                    session.PlayerName)
                                {
                                    PlayerConnections.Remove(
                                        connectionId);
                                }
                            }
                        }

                        if (ActivePlayerConnections.TryGetValue(
                            session.PlayerName,
                            out var activeConnectionId))
                        {
                            if (session.ConnectionId ==
                                activeConnectionId)
                            {
                                ActivePlayerConnections.Remove(
                                    session.PlayerName);
                            }
                        }

                        PlayerSessions.Remove(token);
                    }
                }
            }
        }

    }
}