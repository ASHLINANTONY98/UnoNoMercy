using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;
using UnoNoMercy.GameEngine.Services;

var gameService = new GameService();

var game = new Game
{
    Players =
    {
        new Player { Name = "Ashlin" },
        new Player { Name = "Rahul" }
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
// ASHLIN GETS RED REVERSE
// ========================================

var ashlinReverse = new Card
{
    Color = CardColor.Red,
    Type = CardType.Reverse
};

game.Players[0].Hand.Add(ashlinReverse);

// Extra card so Ashlin doesn't win
game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// ========================================
// RAHUL GETS BLUE REVERSE
// ========================================

var rahulReverse = new Card
{
    Color = CardColor.Blue,
    Type = CardType.Reverse
};

game.Players[1].Hand.Add(rahulReverse);

// Extra card so Rahul doesn't win
game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 3
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== 2 PLAYER REVERSE TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS REVERSE
// ========================================

var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinReverse.Id);

Console.WriteLine("=== ASHLIN PLAYS REVERSE ===");

Console.WriteLine(
    $"Success: {result1.Success}");

Console.WriteLine(
    $"Message: {result1.Message}");

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction AFTER: {game.Direction}");

Console.WriteLine();

// ========================================
// RAHUL PLAYS REVERSE
// ========================================

var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulReverse.Id);

Console.WriteLine("=== RAHUL PLAYS REVERSE ===");

Console.WriteLine(
    $"Success: {result2.Success}");

Console.WriteLine(
    $"Message: {result2.Message}");

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction AFTER: {game.Direction}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine();

Console.WriteLine("1. Before");
Console.WriteLine("Current Player: Ashlin");
Console.WriteLine("Direction: 1");

Console.WriteLine();

Console.WriteLine("2. Ashlin plays Reverse");
Console.WriteLine("Direction AFTER: -1");
Console.WriteLine("Current Player AFTER: Rahul");

Console.WriteLine();

Console.WriteLine("3. Rahul plays Reverse");
Console.WriteLine("Direction AFTER: 1");
Console.WriteLine("Current Player AFTER: Ashlin");