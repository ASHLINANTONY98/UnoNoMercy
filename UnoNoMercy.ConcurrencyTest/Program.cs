using System.Net.Http.Json;

Console.WriteLine(
    "=== CONCURRENT START GAME TEST ===");

Console.WriteLine();

var sessionToken =
    "645e5e5b-8a90-425a-b405-33db77670fab";

using var http =
    new HttpClient();

http.BaseAddress =
    new Uri("https://localhost:7285");

Console.WriteLine(
    "Sending TWO StartGame requests simultaneously...");

var before =
    await http.PostAsJsonAsync(
        "/api/Game/state",
        new
        {
            SessionToken = sessionToken
        });

Console.WriteLine();
Console.WriteLine("=== BEFORE ===");

Console.WriteLine(
    await before.Content.ReadAsStringAsync());
var request1 =
    http.PostAsJsonAsync(
        "/api/Game/next-turn",
        new
        {
            SessionToken = sessionToken
        });

var request2 =
    http.PostAsJsonAsync(
        "/api/Game/next-turn",
        new
        {
            SessionToken = sessionToken
        });

var responses =
    await Task.WhenAll(
        request1,
        request2);

Console.WriteLine();

Console.WriteLine(
    "=== RESPONSE 1 ===");

Console.WriteLine(
    await responses[0]
        .Content
        .ReadAsStringAsync());

Console.WriteLine();

Console.WriteLine(
    "=== RESPONSE 2 ===");

Console.WriteLine(
    await responses[1]
        .Content
        .ReadAsStringAsync());

Console.WriteLine();

Console.WriteLine(
    $"Status 1: {responses[0].StatusCode}");

Console.WriteLine(
    $"Status 2: {responses[1].StatusCode}");

var after =
    await http.PostAsJsonAsync(
        "/api/Game/state",
        new
        {
            SessionToken = sessionToken
        });

Console.WriteLine();

Console.WriteLine(
    "=== AFTER ===");

Console.WriteLine(
    await after.Content.ReadAsStringAsync());

Console.WriteLine();

Console.WriteLine(
    "Press ENTER to exit.");

Console.ReadLine();