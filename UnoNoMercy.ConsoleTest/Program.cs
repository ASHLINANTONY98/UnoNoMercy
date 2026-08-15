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

// Top card
game.DiscardPile.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 5
});

// Give Ashlin Skip Everyone
var skipEveryoneCard = new Card
{
    Color = CardColor.Red,
    Type = CardType.SkipEveryone
};

game.Players[0].Hand.Add(skipEveryoneCard);

// Give Ashlin another card so he cannot win
game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 5
});

Console.WriteLine("=== SKIP EVERYONE TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Top Card: {gameService.GetTopCard(game)}");

Console.WriteLine();

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    skipEveryoneCard.Id);

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
Console.WriteLine("Ashlin plays Skip Everyone");
Console.WriteLine("Rahul should be skipped");
Console.WriteLine("Arun should be skipped");
Console.WriteLine("After: Ashlin");