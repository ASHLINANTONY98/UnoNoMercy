using FluentAssertions;
using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using UnoNoMercy.Api.Tests.Infrastructure;

namespace UnoNoMercy.Api.Tests;

public class GameCreationTests
    : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public GameCreationTests(
        TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task CreateGame_WithValidPlayer_ReturnsOk()
    {
        // Arrange + Act

        var response =
            await _client.PostAsJsonAsync(
                "/api/Game/create",
                new
                {
                    playerName = "Ashlin"
                });

        // Assert

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);

        var content =
            await response.Content.ReadAsStringAsync();

        content.Should()
            .Contain("gameId");

        content.Should()
            .Contain("roomCode");
    }

    [Fact]
    public async Task CreateGame_EmptyPlayer_ReturnsBadRequest()
    {
        var response =
            await _client.PostAsJsonAsync(
                "/api/Game/create",
                new
                {
                    playerName = ""
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);

        var content =
            await response.Content.ReadAsStringAsync();

        content.Should()
            .Contain("Player name is required");
    }

    
}