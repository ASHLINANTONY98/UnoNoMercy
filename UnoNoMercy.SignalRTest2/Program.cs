using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("=== SIGNALR ROOM TEST TWO===");
Console.WriteLine();

var roomCode = "ZXYNUB";
var playerName = "Rahul";

RoomJoinedResponse? roomJoined = null;

var connection = new HubConnectionBuilder()
    .WithUrl("https://localhost:7285/gamehub")
    .WithAutomaticReconnect()
    .Build();

connection.On<RoomJoinedResponse>(
    "RoomJoined",
    data =>
    {
        Console.WriteLine(
            "=== ROOM JOINED ===");

        Console.WriteLine(
            $"Room: {data.RoomCode}");

        Console.WriteLine(
            $"Player: {data.PlayerName}");

        Console.WriteLine(
            $"Session Token: {data.SessionToken}");

        roomJoined = data;
    });

connection.On<object>(
    "PlayerJoined",
    data =>
    {
        Console.WriteLine();
        Console.WriteLine(
            "=== PLAYER JOINED EVENT ===");
        Console.WriteLine(data);
    });

connection.On<object>(
    "GameStateUpdated",
    data =>
    {
        Console.WriteLine();
        Console.WriteLine(
            "=== GAME STATE UPDATED ===");
        Console.WriteLine(data);
    });

connection.Closed += async error =>
{
    Console.WriteLine();
    Console.WriteLine(
        "SignalR connection closed.");

    if (error != null)
    {
        Console.WriteLine(
            $"Error: {error.Message}");
    }

    await Task.CompletedTask;
};

try
{
    Console.WriteLine(
        "Connecting to SignalR...");

    await connection.StartAsync();

    Console.WriteLine(
        "Connected successfully.");

    Console.WriteLine();

    Console.WriteLine(
        $"Joining room: {roomCode}");

    Console.WriteLine(
        $"Player: {playerName}");

    await connection.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    if (roomJoined == null)
    {
        Console.WriteLine(
            "Session information was not received.");

        return;
    }

    Console.WriteLine();

    Console.WriteLine(
        "Checking session identity...");

    var sessionPlayer =
        await connection.InvokeAsync<object>(
            "GetSessionPlayer",
            roomJoined.SessionToken);

    Console.WriteLine();

    Console.WriteLine(
        "=== SESSION PLAYER ===");

    Console.WriteLine(sessionPlayer);

    Console.WriteLine();

    Console.WriteLine(
        "Checking current connection identity...");

    var myPlayer =
        await connection.InvokeAsync<object>(
            "GetMyPlayer");

    Console.WriteLine(
        "=== MY PLAYER ===");

    Console.WriteLine(myPlayer);

    Console.WriteLine();

    Console.WriteLine(
        "JoinRoom call completed.");

    Console.WriteLine();

    Console.WriteLine(
        "Press ENTER to disconnect.");

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine();

    Console.WriteLine(
        "=== SIGNALR TEST FAILED ===");

    Console.WriteLine(ex.Message);
}
finally
{
    await connection.DisposeAsync();
}


// DTO must be AFTER the top-level statements
public class RoomJoinedResponse
{
    public string RoomCode { get; set; }
        = string.Empty;

    public string PlayerName { get; set; }
        = string.Empty;

    public string SessionToken { get; set; }
        = string.Empty;
}