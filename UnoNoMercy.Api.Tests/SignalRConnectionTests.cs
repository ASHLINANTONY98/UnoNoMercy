using FluentAssertions;
using Microsoft.AspNetCore.SignalR.Client;

namespace UnoNoMercy.Api.Tests;

public class SignalRConnectionTests
{
    [Fact]
    public async Task
        CanConnectToGameHub()
    {
        var connection =
            new HubConnectionBuilder()
                .WithUrl(
                    "http://localhost:5270/gamehub")
                .Build();

        await connection.StartAsync();

        connection.State
            .Should()
            .Be(
                HubConnectionState.Connected);

        await connection.DisposeAsync();
    }
}