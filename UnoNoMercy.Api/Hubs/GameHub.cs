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

            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                game.RoomCode);

            _gameManager.PlayerConnections[
                Context.ConnectionId] =
                player.Name;

            await Clients.Caller.SendAsync(
                "RoomJoined",
                new
                {
                    RoomCode = game.RoomCode,
                    PlayerName = player.Name
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