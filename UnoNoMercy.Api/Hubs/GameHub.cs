using Microsoft.AspNetCore.SignalR;
using UnoNoMercy.Api.Services;

namespace UnoNoMercy.Api.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameManager _gameManager;
        private readonly PlayerSessionService _playerSessionService;

        public GameHub(
            GameManager gameManager,
            PlayerSessionService playerSessionService)
        {
            _gameManager = gameManager;
            _playerSessionService = playerSessionService;
        }

        public async Task JoinRoom(
            string roomCode,
            string playerName)
        {
            Console.WriteLine(
                $"[SignalR] JoinRoom called: {playerName} / {roomCode}");

            if (string.IsNullOrWhiteSpace(roomCode))
            {
                throw new HubException(
                    "Room code is required.");
            }

            if (string.IsNullOrWhiteSpace(playerName))
            {
                throw new HubException(
                    "Player name is required.");
            }

            var gameEntry =
                _gameManager.Games
                    .FirstOrDefault(x =>
                        x.Value.RoomCode
                            .Equals(
                                roomCode,
                                StringComparison.OrdinalIgnoreCase));

            if (gameEntry.Value == null)
            {
                throw new HubException(
                    "Room not found.");
            }

            var game = gameEntry.Value;

            var player = game.Players
                .FirstOrDefault(x =>
                    x.Name.Equals(
                        playerName,
                        StringComparison.OrdinalIgnoreCase));

            if (player == null)
            {
                throw new HubException(
                    "Player is not a member of this room.");
            }

            if (player.IsEliminated)
            {
                throw new HubException(
                    "Eliminated players cannot join.");
            }

            // Add connection to SignalR room
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                game.RoomCode);

            // Associate connection with player
            _gameManager.PlayerConnections[
                Context.ConnectionId] =
                player.Name;

            Console.WriteLine(
                $"[SignalR] Sending RoomJoined: {player.Name}");

            var sessionToken =
                Guid.NewGuid().ToString();

            _gameManager.PlayerSessions[
                sessionToken] =
                new PlayerSession
                {
                    GameId = gameEntry.Key,
                    RoomCode = game.RoomCode,
                    PlayerName = player.Name,
                    ConnectionId = Context.ConnectionId,
                    LastActivityUtc = DateTime.UtcNow
                };

            // Tell the joining player
            await Clients.Caller.SendAsync(
                "RoomJoined",
                new
                {
                    RoomCode = game.RoomCode,
                    PlayerName = player.Name,
                    SessionToken = sessionToken
                });

            // Tell everyone in the room
            await Clients.OthersInGroup(game.RoomCode)
                .SendAsync(
                    "PlayerJoined",
                    new
                    {
                        PlayerName = player.Name
                    });
        }

        public Task<object> GetMyPlayer()
        {
            if (!_gameManager.PlayerConnections.TryGetValue(
                Context.ConnectionId,
                out var playerName))
            {
                throw new HubException(
                    "You are not connected to a player.");
            }

            return Task.FromResult<object>(
                new
                {
                    PlayerName = playerName,
                    ConnectionId = Context.ConnectionId
                });
        }

        public Task<object> GetSessionPlayer(
            string sessionToken)
        {
            PlayerSession session;

            try
            {
                session =
                    _playerSessionService.GetRequiredSession(
                        sessionToken);
            }
            catch (ArgumentException ex)
            {
                throw new HubException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new HubException(ex.Message);
            }

            return Task.FromResult<object>(
                new
                {
                    session.GameId,
                    session.RoomCode,
                    session.PlayerName,
                    session.ConnectionId
                });
        }

        public async Task<object> ResumeSession(
            string sessionToken)
        {
            PlayerSession session;

            try
            {
                session =
                    _playerSessionService.GetRequiredSession(
                        sessionToken);
            }
            catch (ArgumentException ex)
            {
                throw new HubException(ex.Message);
            }
            catch (UnauthorizedAccessException ex)
            {
                throw new HubException(ex.Message);
            }

            if (!_gameManager.Games.TryGetValue(
                session.GameId,
                out var game))
            {
                throw new HubException(
                    "Game not found.");
            }

            if (!game.RoomCode.Equals(
                session.RoomCode,
                StringComparison.OrdinalIgnoreCase))
            {
                throw new HubException(
                    "Session does not belong to this room.");
            }

            var player = game.Players
                .FirstOrDefault(x =>
                    x.Name.Equals(
                        session.PlayerName,
                        StringComparison.OrdinalIgnoreCase));

            if (player == null)
            {
                throw new HubException(
                    "Player is not a member of this room.");
            }

            if (player.IsEliminated)
            {
                throw new HubException(
                    "Eliminated players cannot reconnect.");
            }

            // Remove the previous connection mapping
            if (!string.IsNullOrWhiteSpace(
                session.ConnectionId))
            {
                _gameManager.PlayerConnections
                    .Remove(session.ConnectionId);
            }

            // Add the new connection to the SignalR room
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                game.RoomCode);

            // Associate the new connection with the player
            _gameManager.PlayerConnections[
                Context.ConnectionId] =
                player.Name;

            // Update the session with the new connection
            session.ConnectionId =
                Context.ConnectionId;

            session.LastActivityUtc =
                DateTime.UtcNow;

            return new
            {
                session.GameId,
                session.RoomCode,
                session.PlayerName,
                session.ConnectionId
            };
        }

        public override async Task OnDisconnectedAsync(
            Exception? exception)
        {
            _gameManager.PlayerConnections
                .Remove(Context.ConnectionId);

            await base.OnDisconnectedAsync(
                exception);
        }
    }
}