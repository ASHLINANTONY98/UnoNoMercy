namespace UnoNoMercy.Api.Services
{
    public class PlayerSessionService
    {
        private readonly GameManager _gameManager;

        private static readonly TimeSpan SessionExpiration =
            TimeSpan.FromHours(24);

        public PlayerSessionService(
            GameManager gameManager)
        {
            _gameManager = gameManager;
        }

        public PlayerSession GetRequiredSession(
            string sessionToken)
        {
            if (string.IsNullOrWhiteSpace(sessionToken))
            {
                throw new ArgumentException(
                    "Session token is required.");
            }

            lock (_gameManager.SyncRoot)
            {
                if (!_gameManager.TryGetSession(
                    sessionToken,
                    out var session))
                {
                    throw new UnauthorizedAccessException(
                        "Invalid session token.");
                }

                if (DateTime.UtcNow - session.LastActivityUtc
                    > SessionExpiration)
                {
                    throw new UnauthorizedAccessException(
                        "Session has expired.");
                }

                session.LastActivityUtc =
                    DateTime.UtcNow;

                return session;
            }
        }
    }
}