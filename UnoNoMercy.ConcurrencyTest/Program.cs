using Microsoft.AspNetCore.SignalR.Client;
using System.Net.Http.Json;
using UnoNoMercy.GameEngine.DTOs;

Console.WriteLine("=== CONCURRENT PLAY CARD TEST ===");
Console.WriteLine();

Console.WriteLine("Press ENTER to start...");
Console.ReadLine();

var roomCode = "HF95JW";
var playerName = "Ashlin";

var connection1 =
    new HubConnectionBuilder()
        .WithUrl("https://localhost:7285/gamehub")
        .Build();

var connection2 =
    new HubConnectionBuilder()
        .WithUrl("https://localhost:7285/gamehub")
        .Build();

try
{
    // ============================================================
    // CONNECTION 1
    // ============================================================

    Console.WriteLine("Connecting Connection 1...");

    var joined1 =
        new TaskCompletionSource<RoomJoinedResponse>();

    connection1.On<RoomJoinedResponse>(
        "RoomJoined",
        data =>
        {
            joined1.TrySetResult(data);
        });

    await connection1.StartAsync();

    await connection1.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    var session1 = await joined1.Task;

    Console.WriteLine(
        $"Connection 1 Session: {session1.SessionToken}");

    // ============================================================
    // CONNECTION 2
    // ============================================================

    Console.WriteLine();
    Console.WriteLine("Connecting Connection 2...");

    var joined2 =
        new TaskCompletionSource<RoomJoinedResponse>();

    connection2.On<RoomJoinedResponse>(
        "RoomJoined",
        data =>
        {
            joined2.TrySetResult(data);
        });

    await connection2.StartAsync();

    await connection2.InvokeAsync(
        "JoinRoom",
        roomCode,
        playerName);

    var session2 = await joined2.Task;

    Console.WriteLine(
        $"Connection 2 Session: {session2.SessionToken}");

    // ============================================================
    // GET ASHLIN'S HAND
    // ============================================================

    using var http = new HttpClient();

    http.BaseAddress =
        new Uri("https://localhost:7285");

    http.DefaultRequestVersion =
        new Version(2, 0);

    Console.WriteLine();
    Console.WriteLine("Getting Ashlin's hand...");

    var handResponse =
        await http.PostAsJsonAsync(
            "/api/Game/hand",
            new
            {
                SessionToken = session1.SessionToken
            });

    handResponse.EnsureSuccessStatusCode();

    var hand =
        await handResponse.Content
            .ReadFromJsonAsync<List<CardDto>>();

    Console.WriteLine();
    Console.WriteLine("Getting current game state...");

    var stateResponse =
        await http.PostAsJsonAsync(
            "/api/Game/state",
            new
            {
                SessionToken = session1.SessionToken
            });

    stateResponse.EnsureSuccessStatusCode();

    var state =
        await stateResponse.Content
            .ReadFromJsonAsync<GameStateDto>();

    if (state == null)
    {
        Console.WriteLine("ERROR: Could not get game state.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("=== CURRENT GAME STATE ===");
    Console.WriteLine($"Current Player: {state.CurrentPlayer}");
    Console.WriteLine($"Top Card: {state.TopCard}");

    if (hand == null || hand.Count == 0)
    {
        Console.WriteLine("ERROR: Ashlin has no cards.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine("=== ASHLIN HAND ===");

    foreach (var card in hand)
    {
        Console.WriteLine(
            $"{card.Id} - {card.DisplayText}");
    }

    // ============================================================
    // CHOOSE ONE CARD
    // ============================================================
    Console.WriteLine();
    Console.WriteLine("=== CHOOSE A PLAYABLE CARD ===");

    Console.Write("Enter Card ID: ");
    var cardId = Console.ReadLine();

    if (string.IsNullOrWhiteSpace(cardId))
    {
        Console.WriteLine("No card selected.");
        return;
    }

    var cardToPlay =
        hand.FirstOrDefault(x =>
            x.Id.Equals(
                cardId,
                StringComparison.OrdinalIgnoreCase));

    if (cardToPlay == null)
    {
        Console.WriteLine("Card ID is not in Ashlin's hand.");
        return;
    }

    Console.WriteLine();
    Console.WriteLine(
        $"Testing card: {cardToPlay.DisplayText}");

    Console.WriteLine(
        $"Card ID: {cardToPlay.Id}");

    Console.WriteLine();
    Console.WriteLine(
        $"Testing card: {cardToPlay.DisplayText}");

    Console.WriteLine(
        $"Card ID: {cardToPlay.Id}");

    // ============================================================
    // CONCURRENT PLAY
    // ============================================================

    Console.WriteLine();
    Console.WriteLine(
        "Sending TWO PlayCard requests simultaneously...");

    var request1 =
        http.PostAsJsonAsync(
            "/api/Game/play-card",
            new
            {
                SessionToken = session1.SessionToken,
                CardId = cardToPlay.Id
            });

    var request2 =
        http.PostAsJsonAsync(
            "/api/Game/play-card",
            new
            {
                SessionToken = session2.SessionToken,
                CardId = cardToPlay.Id
            });

    var responses =
        await Task.WhenAll(
            request1,
            request2);

    // ============================================================
    // RESULTS
    // ============================================================

    Console.WriteLine();
    Console.WriteLine("=== RESULT 1 ===");

    Console.WriteLine(
        await responses[0].Content.ReadAsStringAsync());

    Console.WriteLine();
    Console.WriteLine("=== RESULT 2 ===");

    Console.WriteLine(
        await responses[1].Content.ReadAsStringAsync());

    Console.WriteLine();
    Console.WriteLine("=== HTTP STATUS ===");

    Console.WriteLine(
        $"Request 1: {responses[0].StatusCode}");

    Console.WriteLine(
        $"Request 2: {responses[1].StatusCode}");

    Console.WriteLine();
    Console.WriteLine(
        "Concurrent test completed.");

    Console.WriteLine();
    Console.WriteLine(
        "Press ENTER to exit.");

    Console.ReadLine();
}
catch (Exception ex)
{
    Console.WriteLine();
    Console.WriteLine("================================");
    Console.WriteLine("=== CONCURRENCY TEST FAILED ===");
    Console.WriteLine("================================");
    Console.WriteLine();

    Console.WriteLine(ex.ToString());

    Console.WriteLine();
    Console.WriteLine("Press ENTER to exit...");
    Console.ReadLine();
}
finally
{
    await connection1.DisposeAsync();
    await connection2.DisposeAsync();
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

public class CardDto
{
    public string Id { get; set; }
        = string.Empty;

    public string DisplayText { get; set; }
        = string.Empty;
}