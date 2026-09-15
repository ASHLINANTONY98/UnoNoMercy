using System.Net.Http.Json;
using System.Text.Json;

namespace UnoNoMercy.Api.Tests.Helpers;

public static class GameTestHelper 
{
    public static async Task<string> CreateRoomAsync(
        HttpClient client,
        string playerName = "Ashlin")
    {
        var response =
            await client.PostAsJsonAsync(
                "/api/Game/create",
                new
                {
                    playerName
                });

        response.EnsureSuccessStatusCode();

        var content =
            await response.Content
                .ReadFromJsonAsync<JsonElement>();

        return content
            .GetProperty("roomCode")
            .GetString()!;
    }
}