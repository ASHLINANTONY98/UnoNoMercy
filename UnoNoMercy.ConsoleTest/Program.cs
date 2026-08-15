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

// Ashlin is the current player
game.CurrentPlayerIndex = 0;

// Normal clockwise direction
game.Direction = 1;
game.HasStarted = true;

// Top card
game.DiscardPile.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 5
});

// Give Ashlin a Red Skip
var skipCard = new Card
{
    Color = CardColor.Red,
    Type = CardType.Skip
};

game.Players[0].Hand.Add(skipCard);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 5
});

Console.WriteLine("=== SKIP TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Top Card: {gameService.GetTopCard(game)}");

Console.WriteLine();

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    skipCard.Id);

Console.WriteLine(
    $"Play Result: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine();

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Turn Advance: {game.TurnAdvance}");

Console.WriteLine();

Console.WriteLine("=== EXPECTED ===");
Console.WriteLine("Before: Ashlin");
Console.WriteLine("Ashlin plays Skip");
Console.WriteLine("Rahul should be skipped");
Console.WriteLine("After: Arun");