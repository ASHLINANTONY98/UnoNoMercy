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
        new Player { Name = "Arun" },
        new Player { Name = "Neha" }
    }
};

game.CurrentPlayerIndex = 0;
game.Direction = 1;
game.HasStarted = true;

// ========================================
// ELIMINATE RAHUL
// ========================================

game.Players[1].IsEliminated = true;

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
// ASHLIN GETS SKIP EVERYONE
// ========================================

var skipEveryone = new Card
{
    Color = CardColor.Red,
    Type = CardType.SkipEveryone
};

game.Players[0].Hand.Add(skipEveryone);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// ========================================
// ARUN
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// NEHA
// ========================================

game.Players[3].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 7
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== SKIP EVERYONE ELIMINATED PLAYER TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine(
    $"Turn Advance BEFORE: {game.TurnAdvance}");

Console.WriteLine(
    $"Rahul Eliminated: {game.Players[1].IsEliminated}");

Console.WriteLine(
    $"Active Players: {game.Players.Count(p => !p.IsEliminated)}");

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

Console.WriteLine("Rahul is eliminated before the card is played.");

Console.WriteLine();

Console.WriteLine("Ashlin plays Skip Everyone.");

Console.WriteLine("Rahul is ignored because he is eliminated.");
Console.WriteLine("Arun should be skipped.");
Console.WriteLine("Neha should be skipped.");

Console.WriteLine();

Console.WriteLine("Current Player AFTER: Ashlin");
Console.WriteLine("Direction AFTER: 1");
Console.WriteLine("Turn Advance AFTER: 1");