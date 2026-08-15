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
// ASHLIN: WILD DRAW SIX (+6)
// ========================================

var ashlinDrawSix = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawSix
};

game.Players[0].Hand.Add(ashlinDrawSix);

// Extra card
game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
});

// ========================================
// RAHUL: WILD DRAW TEN (+10)
// ========================================

var rahulDrawTen = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawTen
};

game.Players[1].Hand.Add(rahulDrawTen);

// Extra card
game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 3
});

// ========================================
// ARUN STARTING HAND
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 7
});

// ========================================
// CONTROLLED DECK
// ========================================

// We need at least 16 cards because
// Arun must receive the complete penalty.

for (int i = 1; i <= 16; i++)
{
    game.Deck.Add(new Card
    {
        Color = CardColor.Red,
        Type = CardType.Number,
        Number = i % 10
    });
}

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== +6 -> +10 PENALTY TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Pending Draw BEFORE: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value BEFORE: {game.CurrentStackValue}");

Console.WriteLine(
    $"Arun Hand BEFORE: {game.Players[2].Hand.Count}");

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
// RAHUL PLAYS +10
// ========================================

var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulDrawTen.Id);

Console.WriteLine("=== RAHUL STACKS +10 ===");

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

Console.WriteLine();

// ========================================
// ARUN TAKES PENALTY
// ========================================

var result3 = gameService.DrawPlayerCard(
    game,
    "Arun");

Console.WriteLine("=== ARUN TAKES +16 PENALTY ===");

Console.WriteLine(
    $"Success: {result3.Success}");

Console.WriteLine(
    $"Message: {result3.Message}");

Console.WriteLine(
    $"Current Player AFTER DRAW: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Arun Hand AFTER: {game.Players[2].Hand.Count}");

Console.WriteLine(
    $"Pending Draw AFTER: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value AFTER: {game.CurrentStackValue}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine();

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin plays +6");
Console.WriteLine("Pending Draw: 6");
Console.WriteLine("Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("Rahul plays +10");
Console.WriteLine("Pending Draw: 16");
Console.WriteLine("Current Player: Arun");

Console.WriteLine();

Console.WriteLine("Arun takes +16 penalty");
Console.WriteLine("Arun receives 16 cards");
Console.WriteLine("Pending Draw: 0");
Console.WriteLine("Stack Value: 0");
Console.WriteLine("Current Player: Ashlin");