using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("=== SIGNALR RESUME SESSION TEST ===");
Console.WriteLine();

var roomCode = "HBPKY4"; // CHANGE THIS
var playerName = "Ashlin";

try
{
    // ============================================================
    // CONNECTION 1
    // ============================================================

    Console.WriteLine("=== CONNECTION 1 ===");

    var roomJoinedSource =
        new TaskCompletionSource<RoomJoinedResponse>();

    var connection1 =
        new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/gamehub")
            .Build();

    connection1.On<RoomJoinedResponse>(
        "RoomJoined",
        data =>
        {
            Console.WriteLine();
            Console.WriteLine("=== ROOM JOINED ===");
            Console.WriteLine(
                $"Room: {data.RoomCode}");
            Console.WriteLine(
                $"Player: {data.PlayerName}");
            Console.WriteLine(
                $"Session Token: {data.SessionToken}");

            roomJoinedSource.TrySetResult(data);
        });

    await connection1.StartAsync();

    Console.WriteLine(
        "Connection 1 connected.");

    await connection1.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    var roomJoined =
        await roomJoinedSource.Task;

    var firstPlayer =
        await connection1.InvokeAsync<ConnectionPlayerResponse>(
            "GetMyPlayer");

    Console.WriteLine();
    Console.WriteLine("=== FIRST CONNECTION IDENTITY ===");
    Console.WriteLine(
        $"Player: {firstPlayer.PlayerName}");
    Console.WriteLine(
        $"Connection ID: {firstPlayer.ConnectionId}");

    var sessionToken =
        roomJoined.SessionToken;

    // ============================================================
    // DISCONNECT CONNECTION 1
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Disconnecting connection 1...");

    await connection1.DisposeAsync();

    Console.WriteLine(
        "Connection 1 disconnected.");

    // ============================================================
    // CONNECTION 2
    // ============================================================

    Console.WriteLine();
    Console.WriteLine("=== CONNECTION 2 ===");

    var connection2 =
        new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/gamehub")
            .Build();

    await connection2.StartAsync();

    Console.WriteLine(
        "Connection 2 connected.");

    // ============================================================
    // RESUME SESSION
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Resuming session...");

    var resumedSession =
        await connection2.InvokeAsync<ResumeSessionResponse>(
            "ResumeSession",
            sessionToken);

    Console.WriteLine();
    Console.WriteLine(
        "=== SESSION RESUMED ===");

    Console.WriteLine(
        $"Game ID: {resumedSession.GameId}");

    Console.WriteLine(
        $"Room Code: {resumedSession.RoomCode}");

    Console.WriteLine(
        $"Player: {resumedSession.PlayerName}");

    Console.WriteLine(
        $"New Connection ID: {resumedSession.ConnectionId}");

    // ============================================================
    // VERIFY CURRENT CONNECTION
    // ============================================================

    var secondPlayer =
        await connection2.InvokeAsync<ConnectionPlayerResponse>(
            "GetMyPlayer");

    Console.WriteLine();
    Console.WriteLine(
        "=== SECOND CONNECTION IDENTITY ===");

    Console.WriteLine(
        $"Player: {secondPlayer.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {secondPlayer.ConnectionId}");

    // ============================================================
    // VERIFY CONNECTION ID CHANGED
    // ============================================================

    Console.WriteLine();

    if (firstPlayer.ConnectionId !=
        secondPlayer.ConnectionId)
    {
        Console.WriteLine(
            "SUCCESS: Connection ID changed after reconnect.");
    }
    else
    {
        Console.WriteLine(
            "ERROR: Connection ID did not change.");
    }

    // ============================================================
    // VERIFY SESSION PLAYER
    // ============================================================

    var sessionPlayer =
        await connection2.InvokeAsync<ResumeSessionResponse>(
            "GetSessionPlayer",
            sessionToken);

    Console.WriteLine();
    Console.WriteLine(
        "=== SESSION AFTER RESUME ===");

    Console.WriteLine(
        $"Game ID: {sessionPlayer.GameId}");

    Console.WriteLine(
        $"Room Code: {sessionPlayer.RoomCode}");

    Console.WriteLine(
        $"Player: {sessionPlayer.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {sessionPlayer.ConnectionId}");

    Console.WriteLine();
    Console.WriteLine(
        "Press ENTER to disconnect.");

    Console.ReadLine();

    await connection2.DisposeAsync();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine(
        "=== RESUME SESSION TEST FAILED ===");

    Console.WriteLine(
        ex.Message);
}

public class RoomJoinedResponse
{
    public string RoomCode { get; set; }
        = string.Empty;

    public string PlayerName { get; set; }
        = string.Empty;

    public string SessionToken { get; set; }
        = string.Empty;
}

public class ConnectionPlayerResponse
{
    public string PlayerName { get; set; }
        = string.Empty;

    public string ConnectionId { get; set; }
        = string.Empty;
}

public class ResumeSessionResponse
{
    public Guid GameId { get; set; }

    public string RoomCode { get; set; }
        = string.Empty;

    public string PlayerName { get; set; }
        = string.Empty;

    public string? ConnectionId { get; set; }
}