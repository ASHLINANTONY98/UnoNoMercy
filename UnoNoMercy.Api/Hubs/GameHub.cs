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

            var sessionToken =
                Guid.NewGuid().ToString();

            lock (_gameManager.SyncRoot)
            {
                if (_gameManager.ActivePlayerConnections.TryGetValue(
                    player.Name,
                    out var existingConnectionId))
                {
                    if (existingConnectionId != Context.ConnectionId)
                    {
                        _gameManager.PlayerConnections
                            .Remove(existingConnectionId);
                    }
                }

                _gameManager.PlayerConnections[
                    Context.ConnectionId] =
                    player.Name;

                _gameManager.ActivePlayerConnections[
                    player.Name] =
                    Context.ConnectionId;

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

                _gameManager.ActiveSessionConnections[
                    sessionToken] =
                    Context.ConnectionId;
            }


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

            lock (_gameManager.SyncRoot)
            {
                if (!string.IsNullOrWhiteSpace(
                    session.ConnectionId))
                {
                    _gameManager.PlayerConnections
                        .Remove(session.ConnectionId);
                }

                // Replace the player's previous active connection
                if (_gameManager.ActivePlayerConnections.TryGetValue(
                    player.Name,
                    out var existingPlayerConnectionId))
                {
                    if (existingPlayerConnectionId != Context.ConnectionId)
                    {
                        _gameManager.PlayerConnections
                            .Remove(existingPlayerConnectionId);
                    }
                }

                _gameManager.ActivePlayerConnections[
                    player.Name] =
                    Context.ConnectionId;

                _gameManager.ActiveSessionConnections[
                    sessionToken] =
                    Context.ConnectionId;

                _gameManager.PlayerConnections[
                    Context.ConnectionId] =
                    player.Name;

                session.ConnectionId =
                    Context.ConnectionId;

                session.LastActivityUtc =
                    DateTime.UtcNow;
            }

            // Add the new connection to the SignalR room
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                game.RoomCode);

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
            lock (_gameManager.SyncRoot)
            {
                _gameManager.PlayerConnections
                    .Remove(Context.ConnectionId);

                var sessionEntry =
                    _gameManager.ActiveSessionConnections
                        .FirstOrDefault(x =>
                            x.Value == Context.ConnectionId);

                if (!string.IsNullOrEmpty(sessionEntry.Key))
                {
                    var sessionToken = sessionEntry.Key;

                    if (_gameManager.PlayerSessions.TryGetValue(
                        sessionToken,
                        out var session))
                    {
                        // Only clean up if this is still
                        // the active connection for the session.
                        if (session.ConnectionId ==
                            Context.ConnectionId)
                        {
                            _gameManager.ActiveSessionConnections
                                .Remove(sessionToken);

                            if (_gameManager.ActivePlayerConnections.TryGetValue(
                                session.PlayerName,
                                out var activeConnectionId))
                            {
                                if (activeConnectionId ==
                                    Context.ConnectionId)
                                {
                                    _gameManager.ActivePlayerConnections
                                        .Remove(session.PlayerName);
                                }
                            }
                        }
                    }
                }
            }

            await base.OnDisconnectedAsync(
                exception);
        }

        public async Task BroadcastGameState(
            string roomCode,
            object gameState)
        {
            await Clients.Group(roomCode)
                .SendAsync(
                    "GameStateUpdated",
                    gameState);
        }
    }
}