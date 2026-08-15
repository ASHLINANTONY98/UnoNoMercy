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
// ASHLIN GETS NORMAL WILD
// ========================================

var wildCard = new Card
{
    Color = CardColor.Wild,
    Type = CardType.Wild
};

game.Players[0].Hand.Add(wildCard);

// Extra card so Ashlin doesn't win
game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
});

// Rahul gets a card
game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
});

// Arun gets a card
game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 7
});

// ========================================
// BEFORE
// ========================================

Console.WriteLine("=== WILD WRONG PLAYER TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Active Color BEFORE: {game.ActiveColor}");

Console.WriteLine();

// ========================================
// ASHLIN PLAYS WILD
// ========================================

var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    wildCard.Id);

Console.WriteLine("=== ASHLIN PLAYS WILD ===");

Console.WriteLine(
    $"Success: {result1.Success}");

Console.WriteLine(
    $"Message: {result1.Message}");

Console.WriteLine(
    $"Current Player AFTER PLAY: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Active Color AFTER PLAY: {game.ActiveColor}");

Console.WriteLine();

// ========================================
// ARUN TRIES TO CHOOSE BLUE
// ========================================

var result2 = gameService.ChooseWildColor(
    game,
    "Arun",
    CardColor.Blue);

Console.WriteLine("=== ARUN TRIES TO CHOOSE BLUE ===");

Console.WriteLine(
    $"Success: {result2.Success}");

Console.WriteLine(
    $"Message: {result2.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Active Color: {game.ActiveColor}");

Console.WriteLine();

// ========================================
// RAHUL CHOOSES BLUE
// ========================================

var result3 = gameService.ChooseWildColor(
    game,
    "Rahul",
    CardColor.Blue);

Console.WriteLine("=== RAHUL CHOOSES BLUE ===");

Console.WriteLine(
    $"Success: {result3.Success}");

Console.WriteLine(
    $"Message: {result3.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Active Color: {game.ActiveColor}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");
Console.WriteLine();

Console.WriteLine("Ashlin plays Wild");
Console.WriteLine("Current Player AFTER PLAY: Rahul");
Console.WriteLine("Active Color AFTER PLAY:");

Console.WriteLine();

Console.WriteLine("Arun tries to choose Blue");
Console.WriteLine("Success: False");
Console.WriteLine("Message: Not your turn.");
Console.WriteLine("Current Player: Rahul");
Console.WriteLine("Active Color:");

Console.WriteLine();

Console.WriteLine("Rahul chooses Blue");
Console.WriteLine("Success: True");
Console.WriteLine("Message: Wild color changed to Blue.");
Console.WriteLine("Current Player: Rahul");
Console.WriteLine("Active Color: Blue");