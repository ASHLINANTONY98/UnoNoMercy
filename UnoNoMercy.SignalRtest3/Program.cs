using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine("=== SIGNALR DUPLICATE CONNECTION TEST THREE ===");
Console.WriteLine();

var roomCode = "9JL9LP";
var playerName = "Ashlin";

try
{
    // ============================================================
    // CONNECTION 1
    // ============================================================

    Console.WriteLine("=== CONNECTION 1 ===");

    var connection1 =
        new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/gamehub")
            .Build();

    // ------------------------------------------------------------
    // CONNECTION 1 EVENT LISTENERS
    // ------------------------------------------------------------

    connection1.On<object>(
        "GameStateUpdated",
        data =>
        {
            Console.WriteLine();
            Console.WriteLine(
                "=== CONNECTION 1 - GAME STATE UPDATED ===");

            Console.WriteLine(data);
        });

    connection1.On<object>(
        "GameStarted",
        data =>
        {
            Console.WriteLine();
            Console.WriteLine(
                "=== CONNECTION 1 - GAME STARTED ===");

            Console.WriteLine(data);
        });

    await connection1.StartAsync();

    Console.WriteLine(
        "Connection 1 connected.");

    // ------------------------------------------------------------
    // CONNECTION 1 - JOIN ROOM
    // ------------------------------------------------------------

    var roomJoined1 =
        new TaskCompletionSource<RoomJoinedResponse>();

    connection1.On<RoomJoinedResponse>(
        "RoomJoined",
        data =>
        {
            roomJoined1.TrySetResult(data);
        });

    await connection1.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    var session1 =
        await roomJoined1.Task;

    Console.WriteLine();
    Console.WriteLine(
        $"Ashlin Session Token: {session1.SessionToken}");

    // ------------------------------------------------------------
    // CONNECTION 1 - IDENTITY
    // ------------------------------------------------------------

    var player1 =
        await connection1.InvokeAsync<ConnectionPlayerResponse>(
            "GetMyPlayer");

    Console.WriteLine();
    Console.WriteLine(
        "=== CONNECTION 1 IDENTITY ===");

    Console.WriteLine(
        $"Player: {player1.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {player1.ConnectionId}");

    // ============================================================
    // CONNECTION 2
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "=== CONNECTION 2 ===");

    var connection2 =
        new HubConnectionBuilder()
            .WithUrl("https://localhost:7285/gamehub")
            .Build();

    // ------------------------------------------------------------
    // CONNECTION 2 EVENT LISTENERS
    // ------------------------------------------------------------

    connection2.On<object>(
        "GameStateUpdated",
        data =>
        {
            Console.WriteLine();
            Console.WriteLine(
                "=== CONNECTION 2 - GAME STATE UPDATED ===");

            Console.WriteLine(data);
        });

    connection2.On<object>(
        "GameStarted",
        data =>
        {
            Console.WriteLine();
            Console.WriteLine(
                "=== CONNECTION 2 - GAME STARTED ===");

            Console.WriteLine(data);
        });

    await connection2.StartAsync();

    Console.WriteLine(
        "Connection 2 connected.");

    // ------------------------------------------------------------
    // CONNECTION 2 - JOIN SAME PLAYER
    // ------------------------------------------------------------

    var roomJoined2 =
        new TaskCompletionSource<RoomJoinedResponse>();

    connection2.On<RoomJoinedResponse>(
        "RoomJoined",
        data =>
        {
            roomJoined2.TrySetResult(data);
        });

    await connection2.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    var session2 =
        await roomJoined2.Task;

    // ------------------------------------------------------------
    // CONNECTION 2 - IDENTITY
    // ------------------------------------------------------------

    var player2 =
        await connection2.InvokeAsync<ConnectionPlayerResponse>(
            "GetMyPlayer");

    Console.WriteLine();
    Console.WriteLine(
        "=== CONNECTION 2 IDENTITY ===");

    Console.WriteLine(
        $"Player: {player2.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {player2.ConnectionId}");

    // ============================================================
    // VERIFY DIFFERENT CONNECTIONS
    // ============================================================

    Console.WriteLine();

    if (player1.ConnectionId !=
        player2.ConnectionId)
    {
        Console.WriteLine(
            "SUCCESS: Two different connections exist.");
    }
    else
    {
        Console.WriteLine(
            "ERROR: Connection IDs are the same.");
    }

    // ============================================================
    // DISCONNECT CONNECTION 1
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Disconnecting CONNECTION 1...");

    await connection1.DisposeAsync();

    Console.WriteLine(
        "Connection 1 disconnected.");

    // Give server a moment to process disconnect
    await Task.Delay(500);

    // ============================================================
    // VERIFY CONNECTION 2 STILL WORKS
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Checking CONNECTION 2...");

    var playerAfterDisconnect =
        await connection2.InvokeAsync<ConnectionPlayerResponse>(
            "GetMyPlayer");

    Console.WriteLine();
    Console.WriteLine(
        "=== CONNECTION 2 AFTER CONNECTION 1 DISCONNECT ===");

    Console.WriteLine(
        $"Player: {playerAfterDisconnect.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {playerAfterDisconnect.ConnectionId}");

    if (playerAfterDisconnect.ConnectionId ==
        player2.ConnectionId)
    {
        Console.WriteLine();
        Console.WriteLine(
            "SUCCESS: Connection 2 is still active.");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine(
            "ERROR: Connection 2 is no longer active.");
    }

    // ============================================================
    // VERIFY SESSION 2
    // ============================================================

    var sessionPlayer =
        await connection2.InvokeAsync<ResumeSessionResponse>(
            "GetSessionPlayer",
            session2.SessionToken);

    Console.WriteLine();
    Console.WriteLine(
        "=== SESSION 2 AFTER CONNECTION 1 DISCONNECT ===");

    Console.WriteLine(
        $"Player: {sessionPlayer.PlayerName}");

    Console.WriteLine(
        $"Connection ID: {sessionPlayer.ConnectionId}");

    if (sessionPlayer.ConnectionId ==
        player2.ConnectionId)
    {
        Console.WriteLine();
        Console.WriteLine(
            "SUCCESS: Session still points to Connection 2.");
    }
    else
    {
        Console.WriteLine();
        Console.WriteLine(
            "ERROR: Session connection is incorrect.");
    }

    // ============================================================
    // KEEP CONNECTION 2 ALIVE
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Press ENTER to disconnect Connection 2.");

    Console.ReadLine();

    await connection2.DisposeAsync();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine(
        "=== DUPLICATE CONNECTION TEST FAILED ===");

    Console.WriteLine(
        ex.Message);
}


// ================================================================
// DTOs
// ================================================================

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

    public string SessionToken { get; set; }
        = string.Empty;
}