using Microsoft.AspNetCore.SignalR;

namespace UnoNoMercy.Api.Hubs
{
    public class GameHub : Hub
    {
        public async Task JoinRoom(
            string roomCode)
        {
            await Groups.AddToGroupAsync(
                Context.ConnectionId,
                roomCode);
        }
    }
}