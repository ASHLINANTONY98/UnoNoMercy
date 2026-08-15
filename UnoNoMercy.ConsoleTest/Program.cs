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
// ASHLIN NORMAL CARD
// ========================================

var ashlinCard = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
};

game.Players[0].Hand.Add(ashlinCard);

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
    Number = 7
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== NEXT TURN ELIMINATED PLAYER TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine(
    $"Rahul Eliminated: {game.Players[1].IsEliminated}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS NORMAL CARD
// ========================================

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinCard.Id);

Console.WriteLine("=== ASHLIN PLAYS RED 8 ===");

Console.WriteLine(
    $"Success: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Current Player Index: {game.CurrentPlayerIndex}");

Console.WriteLine(
    $"Direction AFTER: {game.Direction}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin plays Red 8.");

Console.WriteLine();

Console.WriteLine("Rahul is eliminated.");
Console.WriteLine("Rahul must NOT receive the turn.");

Console.WriteLine();

Console.WriteLine("Expected Current Player AFTER: Arun");
Console.WriteLine("Expected Current Player Index: 2");
Console.WriteLine("Expected Direction: 1");