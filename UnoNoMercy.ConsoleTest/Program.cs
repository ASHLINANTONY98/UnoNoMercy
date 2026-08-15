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
game.Direction = -1;
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
// ASHLIN HAND = 3
// ========================================

var ashlinSeven = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 7
};

game.Players[0].Hand.Add(ashlinSeven);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// RAHUL HAND = 4
// ========================================

for (int i = 1; i <= 4; i++)
{
    game.Players[1].Hand.Add(new Card
    {
        Color = CardColor.Blue,
        Type = CardType.Number,
        Number = i
    });
}

// ========================================
// ARUN HAND = 2
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 8
});

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 9
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== 7 SWAP TARGET TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine(
    $"Ashlin cards BEFORE: {game.Players[0].Hand.Count}");

Console.WriteLine(
    $"Rahul cards BEFORE: {game.Players[1].Hand.Count}");

Console.WriteLine(
    $"Arun cards BEFORE: {game.Players[2].Hand.Count}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS 7 AND CHOOSES ARUN
// ========================================

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinSeven.Id,
    "Arun");

Console.WriteLine("=== ASHLIN PLAYS RED 7 ===");

Console.WriteLine(
    $"Success: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine();

Console.WriteLine($"Ashlin cards AFTER: {game.Players[0].Hand.Count}");
Console.WriteLine($"Rahul cards AFTER: {game.Players[1].Hand.Count}");
Console.WriteLine($"Arun cards AFTER: {game.Players[2].Hand.Count}");

Console.WriteLine();

Console.WriteLine("=== ASHLIN HAND AFTER ===");

foreach (var card in game.Players[0].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("=== RAHUL HAND AFTER ===");

foreach (var card in game.Players[1].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("=== ARUN HAND AFTER ===");

foreach (var card in game.Players[2].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine(
    $"Current Player AFTER: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction AFTER: {game.Direction}");

Console.WriteLine();

Console.WriteLine("=== EXPECTED ===");

Console.WriteLine("Ashlin played Red 7 and chose Arun.");

Console.WriteLine();

Console.WriteLine("Ashlin should now have Arun's original hand:");
Console.WriteLine("- Yellow 8");
Console.WriteLine("- Green 9");

Console.WriteLine();

Console.WriteLine("Rahul should be unchanged:");
Console.WriteLine("- Blue 1");
Console.WriteLine("- Blue 2");
Console.WriteLine("- Blue 3");
Console.WriteLine("- Blue 4");

Console.WriteLine();

Console.WriteLine("Arun should now have Ashlin's remaining hand:");
Console.WriteLine("- Blue 2");
Console.WriteLine("- Green 4");

Console.WriteLine();

Console.WriteLine("Expected counts:");
Console.WriteLine("Ashlin: 2");
Console.WriteLine("Rahul: 4");
Console.WriteLine("Arun: 2");

Console.WriteLine();

Console.WriteLine("Current Player AFTER: Arun");
Console.WriteLine("Direction AFTER: -1");