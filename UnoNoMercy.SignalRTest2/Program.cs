using Microsoft.AspNetCore.SignalR.Client;

Console.WriteLine(
    "=== SIGNALR RESUME SESSION TEST ===");

var roomCode = "FGTDLM";
var playerName = "Ashlin";

string? sessionToken =
    "c3682336-3f74-4f77-b3c7-e91a2440e237";
//
// CONNECTION 1
//

var connection1 =
    new HubConnectionBuilder()
        .WithUrl(
            "https://localhost:7285/gamehub")
        .WithAutomaticReconnect()
        .Build();

await connection1.StartAsync();

Console.WriteLine(
    "\n=== CONNECTION 1 ===");

var tcs =
    new TaskCompletionSource<object>();

connection1.On<object>(
    "RoomJoined",
    data =>
    {
        Console.WriteLine(
        $"RoomJoined Event: {data}");
        tcs.TrySetResult(data);
    });

await connection1.InvokeAsync(
    "JoinRoom",
    roomCode,
    playerName);


var identity1 =
    await connection1.InvokeAsync<object>(
        "GetMyPlayer");

Console.WriteLine(
    $"Connection 1 Identity:");
Console.WriteLine(identity1);

//
// DISCONNECT
//

Console.WriteLine(
    "\nDisconnecting Connection 1...");

await connection1.DisposeAsync();

//
// CONNECTION 2
//

var connection2 =
    new HubConnectionBuilder()
        .WithUrl(
            "https://localhost:7285/gamehub")
        .WithAutomaticReconnect()
        .Build();

await connection2.StartAsync();

Console.WriteLine(
    "\n=== CONNECTION 2 ===");

var resumed =
    await connection2.InvokeAsync<object>(
        "ResumeSession",
        sessionToken);

Console.WriteLine(
    "\n=== RESUME RESULT ===");
Console.WriteLine(resumed);

var identity2 =
    await connection2.InvokeAsync<object>(
        "GetMyPlayer");

Console.WriteLine(
    "\n=== NEW CONNECTION IDENTITY ===");
Console.WriteLine(identity2);

Console.WriteLine(
    "\nSUCCESS: Session resumed.");

Console.ReadLine();