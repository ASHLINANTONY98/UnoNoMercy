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
// ASHLIN HAND
// ========================================

var ashlinZero = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 0
};

game.Players[0].Hand.Add(ashlinZero);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// ========================================
// RAHUL HAND
// ========================================

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 3
});

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// ARUN HAND
// ========================================

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 8
});

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 9
});

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Yellow,
    Type = CardType.Number,
    Number = 1
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== 0 PASS REVERSE TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction BEFORE: {game.Direction}");

Console.WriteLine();

Console.WriteLine("=== HANDS BEFORE ===");

Console.WriteLine("Ashlin:");
foreach (var card in game.Players[0].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("Rahul:");
foreach (var card in game.Players[1].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("Arun:");
foreach (var card in game.Players[2].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

// ========================================
// ASHLIN PLAYS 0
// ========================================

var result = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinZero.Id);

Console.WriteLine("=== ASHLIN PLAYS RED 0 ===");

Console.WriteLine(
    $"Success: {result.Success}");

Console.WriteLine(
    $"Message: {result.Message}");

Console.WriteLine();

Console.WriteLine("=== HANDS AFTER ===");

Console.WriteLine("Ashlin:");
foreach (var card in game.Players[0].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("Rahul:");
foreach (var card in game.Players[1].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine("Arun:");
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

// ========================================
// EXPECTED FOR REVERSE DIRECTION
// ========================================

Console.WriteLine("=== EXPECTED FOR DIRECTION -1 ===");

Console.WriteLine("Ashlin plays Red 0.");

Console.WriteLine();

Console.WriteLine("Reverse rotation should be:");

Console.WriteLine("Ashlin gets Arun's original hand.");
Console.WriteLine("Arun gets Rahul's original hand.");
Console.WriteLine("Rahul gets Ashlin's remaining hand.");

Console.WriteLine();

Console.WriteLine("Ashlin AFTER:");
Console.WriteLine("- Yellow 8");
Console.WriteLine("- Yellow 9");
Console.WriteLine("- Yellow 1");

Console.WriteLine();

Console.WriteLine("Rahul AFTER:");
Console.WriteLine("- Blue 2");

Console.WriteLine();

Console.WriteLine("Arun AFTER:");
Console.WriteLine("- Green 3");
Console.WriteLine("- Green 4");

Console.WriteLine();

Console.WriteLine("Current Player AFTER: Arun");
Console.WriteLine("Direction AFTER: -1");