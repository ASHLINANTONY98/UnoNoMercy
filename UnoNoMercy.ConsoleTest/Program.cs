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
// ASHLIN GETS +6
// ========================================

var ashlinDrawSix = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawSix
};

game.Players[0].Hand.Add(ashlinDrawSix);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 2
});

// ========================================
// RAHUL GETS +6
// ========================================

var rahulDrawSix = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawSix
};

game.Players[1].Hand.Add(rahulDrawSix);

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 7
});

// ========================================
// ARUN
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 3
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== EQUAL DRAW STACK TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw BEFORE: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value BEFORE: {game.CurrentStackValue}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS +6
// ========================================

var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinDrawSix.Id);

Console.WriteLine("=== ASHLIN PLAYS +6 ===");

Console.WriteLine(
    $"Success: {result1.Success}");

Console.WriteLine(
    $"Message: {result1.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

// ========================================
// RAHUL PLAYS EQUAL +6
// ========================================

var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulDrawSix.Id);

Console.WriteLine("=== RAHUL PLAYS EQUAL +6 ===");

Console.WriteLine(
    $"Success: {result2.Success}");

Console.WriteLine(
    $"Message: {result2.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin plays +6");
Console.WriteLine("Pending Draw: 6");
Console.WriteLine("Stack Value: 6");
Console.WriteLine("Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("Rahul plays equal +6");
Console.WriteLine("Success: True");
Console.WriteLine("Pending Draw: 12");
Console.WriteLine("Stack Value: 6");
Console.WriteLine("Current Player: Arun");
Console.WriteLine("Direction: 1");