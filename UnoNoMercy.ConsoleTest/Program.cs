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
// TOP CARD
// ========================================

game.DiscardPile.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 5
});

// ========================================
// ASHLIN'S HAND
// ========================================

// Red cards
var redTwo = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 2
};

var redFive = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 5
};

var redSkip = new Card
{
    Color = CardColor.Red,
    Type = CardType.Skip
};

var redEight = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
};

var redDiscardAll = new Card
{
    Color = CardColor.Red,
    Type = CardType.DiscardAll
};

// Different colors
var blueThree = new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 3
};

var greenSeven = new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 7
};

game.Players[0].Hand.Add(redTwo);
game.Players[0].Hand.Add(redFive);
game.Players[0].Hand.Add(redSkip);
game.Players[0].Hand.Add(redEight);
game.Players[0].Hand.Add(redDiscardAll);
game.Players[0].Hand.Add(blueThree);
game.Players[0].Hand.Add(greenSeven);

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== DISCARD ALL EDGE TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Hand BEFORE: {game.Players[0].Hand.Count}");

Console.WriteLine();

Console.WriteLine("Hand BEFORE:");

foreach (var card in game.Players[0].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

// ========================================
// PLAY RED DISCARD ALL
// ========================================

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    redDiscardAll.Id);

Console.WriteLine("=== ASHLIN PLAYS RED DISCARD ALL ===");

Console.WriteLine(
    $"Success: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine();

Console.WriteLine(
    $"Hand AFTER: {game.Players[0].Hand.Count}");

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine();

Console.WriteLine("Remaining cards:");

foreach (var card in game.Players[0].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Hand BEFORE: 7");

Console.WriteLine();

Console.WriteLine("Red cards before:");
Console.WriteLine("Red 2");
Console.WriteLine("Red 5");
Console.WriteLine("Red Skip");
Console.WriteLine("Red 8");
Console.WriteLine("Red Discard All");

Console.WriteLine();

Console.WriteLine("After Discard All:");

Console.WriteLine("All Red cards should be removed.");

Console.WriteLine("Blue 3 should remain.");
Console.WriteLine("Green 7 should remain.");

Console.WriteLine();

Console.WriteLine("Hand AFTER: 2");
Console.WriteLine("Current Player AFTER: Rahul");