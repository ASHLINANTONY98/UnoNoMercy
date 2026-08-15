using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;
using UnoNoMercy.GameEngine.Services;

var gameService = new GameService();

var game = new Game
{
    Players =
    {
        new Player { Name = "Ashlin" },
        new Player { Name = "Rahul" },
        new Player { Name = "Arun" }
    }
};

game.CurrentPlayerIndex = 0;

// REVERSE direction
game.Direction = -1;

game.HasStarted = true;

// ========================================
// TOP CARD
// ========================================

game.DiscardPile.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 5
});

// ========================================
// ASHLIN GETS RED SKIP EVERYONE
// ========================================

var skipEveryone = new Card
{
    Color = CardColor.Red,
    Type = CardType.SkipEveryone
};

game.Players[0].Hand.Add(skipEveryone);

// Extra card so Ashlin doesn't win
game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== SKIP EVERYONE REVERSE TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine(
    $"Turn Advance BEFORE: {game.TurnAdvance}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS SKIP EVERYONE
// ========================================

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    skipEveryone.Id);

Console.WriteLine("=== ASHLIN PLAYS SKIP EVERYONE ===");

Console.WriteLine(
    $"Success: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction AFTER: {game.Direction}");

Console.WriteLine(
    $"Turn Advance AFTER: {game.TurnAdvance}");

Console.WriteLine();


// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Before: Ashlin");
Console.WriteLine("Direction BEFORE: -1");

Console.WriteLine();

Console.WriteLine("Ashlin plays Skip Everyone");
Console.WriteLine("Arun should be skipped");
Console.WriteLine("Rahul should be skipped");

Console.WriteLine();

Console.WriteLine("After: Ashlin");
Console.WriteLine("Direction AFTER: -1");
Console.WriteLine("Turn Advance AFTER: 1");