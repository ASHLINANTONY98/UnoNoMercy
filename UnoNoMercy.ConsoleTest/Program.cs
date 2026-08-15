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
// ASHLIN GETS RED +2
// ========================================

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

// ========================================
// RAHUL GETS NORMAL CARD
// ========================================

var rahulNormal = new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 7
};

game.Players[1].Hand.Add(rahulNormal);

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// ARUN
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 9
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== INVALID DRAW TWO STACKING TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw BEFORE: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value BEFORE: {game.CurrentStackValue}");

Console.WriteLine(
    $"Rahul Hand BEFORE: {game.Players[1].Hand.Count}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS +2
// ========================================

var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinDrawTwo.Id);

Console.WriteLine("=== ASHLIN PLAYS +2 ===");

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
// RAHUL TRIES NORMAL CARD
// ========================================

var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulNormal.Id);

Console.WriteLine("=== RAHUL TRIES NORMAL CARD ===");

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
    $"Rahul Hand: {game.Players[1].Hand.Count}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin plays +2");
Console.WriteLine("Pending Draw: 2");
Console.WriteLine("Stack Value: 2");
Console.WriteLine("Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("Rahul tries normal card");

Console.WriteLine("Success: False");
Console.WriteLine(
    "Message: Must stack a draw card or take 2 cards.");

Console.WriteLine("Current Player: Rahul");
Console.WriteLine("Pending Draw: 2");
Console.WriteLine("Stack Value: 2");
Console.WriteLine("Rahul Hand: 2");