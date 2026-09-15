using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using UnoNoMercy.Api.Tests.Helpers;
using UnoNoMercy.Api.Tests.Infrastructure;
 
namespace UnoNoMercy.Api.Tests;

public class JoinGameTests
    : IClassFixture<TestApplicationFactory>
{
    private readonly HttpClient _client;

    public JoinGameTests(
        TestApplicationFactory factory)
    {
        _client = factory.CreateClient();
    }

    [Fact]
    public async Task JoinGame_WithValidPlayer_ReturnsOk()
    {
        // create room

        var roomCode =
            await GameTestHelper.CreateRoomAsync(
                _client);

        // join room

        var joinResponse =
            await _client.PostAsJsonAsync(
                "/api/Game/join",
                new
                {
                    roomCode,
                    playerName = "Rahul"
                });

        joinResponse.StatusCode
            .Should()
            .Be(HttpStatusCode.OK);
    }

    [Fact]
    public async Task JoinGame_InvalidRoom_ReturnsNotFound()
    {
        var response =
            await _client.PostAsJsonAsync(
                "/api/Game/join",
                new
                {
                    roomCode = "XXXXXX",
                    playerName = "Rahul"
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task JoinGame_DuplicatePlayer_ReturnsBadRequest()
    {
        var roomCode =
            await GameTestHelper.CreateRoomAsync(
                _client);

        var response =
            await _client.PostAsJsonAsync(
                "/api/Game/join",
                new
                {
                    roomCode,
                    playerName = "Ashlin"
                });

        response.StatusCode
            .Should()
            .Be(HttpStatusCode.BadRequest);
    }
}
        