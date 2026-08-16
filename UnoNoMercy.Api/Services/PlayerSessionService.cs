namespace UnoNoMercy.Api.Services
{
    public class PlayerSessionService
    {
        private readonly GameManager _gameManager;

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

            if (!_gameManager.TryGetSession(
                sessionToken,
                out var session))
            {
                throw new UnauthorizedAccessException(
                    "Invalid session token.");
            }

            // Update session activity
            session.LastActivityUtc =
                DateTime.UtcNow;

            return session;
        }
    }
}