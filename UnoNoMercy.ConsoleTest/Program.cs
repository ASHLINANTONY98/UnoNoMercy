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

// Ashlin +2
var ashlinDrawTwo = new Card
{
    Color = CardColor.Red,
    Type = CardType.DrawTwo
};

game.Players[0].Hand.Add(ashlinDrawTwo);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// Rahul +2
var rahulDrawTwo = new Card
{
    Color = CardColor.Blue,
    Type = CardType.DrawTwo
};

game.Players[1].Hand.Add(rahulDrawTwo);

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 7
});

// Arun
game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 3
});

Console.WriteLine("=== EQUAL +2 STACK TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw BEFORE: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value BEFORE: {game.CurrentStackValue}");

Console.WriteLine();

// Ashlin plays +2
var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinDrawTwo.Id);

Console.WriteLine("=== ASHLIN PLAYS +2 ===");

Console.WriteLine($"Success: {result1.Success}");
Console.WriteLine($"Message: {result1.Message}");
Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");
Console.WriteLine($"Pending Draw: {game.PendingDrawCount}");
Console.WriteLine($"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

// Rahul plays equal +2
var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulDrawTwo.Id);

Console.WriteLine("=== RAHUL PLAYS EQUAL +2 ===");

Console.WriteLine($"Success: {result2.Success}");
Console.WriteLine($"Message: {result2.Message}");
Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");
Console.WriteLine($"Pending Draw: {game.PendingDrawCount}");
Console.WriteLine($"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin plays +2");
Console.WriteLine("Pending Draw: 2");
Console.WriteLine("Stack Value: 2");
Console.WriteLine("Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("Rahul plays equal +2");
Console.WriteLine("Success: True");
Console.WriteLine("Pending Draw: 4");
Console.WriteLine("Stack Value: 2");
Console.WriteLine("Current Player: Arun");