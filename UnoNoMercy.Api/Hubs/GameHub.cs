using Microsoft.AspNetCore.SignalR;
using UnoNoMercy.Api.Services;

namespace UnoNoMercy.Api.Hubs
{
    public class GameHub : Hub
    {
        private readonly GameManager _gameManager;

        public GameHub(GameManager gameManager)
        {
            _gameManager = gameManager;
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

            // Tell the joining player
            await Clients.Caller.SendAsync(
                "RoomJoined",
                new
                {
                    RoomCode = game.RoomCode,
                    PlayerName = player.Name
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