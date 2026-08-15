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

// ========================================
// RAHUL STARTING HAND
// ========================================

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Blue,
    Type = CardType.Number,
    Number = 2
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
// BEFORE
// ========================================

Console.WriteLine("=== NORMAL WILD COLOR TEST ===");
Console.WriteLine();

Console.WriteLine(
    $"Current Player BEFORE: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Top Card: {gameService.GetTopCard(game)}");

Console.WriteLine(
    $"Active Color BEFORE: {game.ActiveColor}");

Console.WriteLine(
    $"Pending Draw BEFORE: {game.PendingDrawCount}");

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

Console.WriteLine(
    $"Pending Draw AFTER PLAY: {game.PendingDrawCount}");

Console.WriteLine();


// ========================================
// RAHUL TRIES INVALID COLOR
// ========================================

var result2 = gameService.ChooseWildColor(
    game,
    "Rahul",
    CardColor.Wild);

Console.WriteLine("=== RAHUL TRIES TO CHOOSE WILD ===");

Console.WriteLine(
    $"Success: {result2.Success}");

Console.WriteLine(
    $"Message: {result2.Message}");

Console.WriteLine(
    $"Active Color: {game.ActiveColor}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

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
    $"Current Player AFTER COLOR: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Active Color AFTER COLOR: {game.ActiveColor}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine();


// ========================================
// EXPECTED
// ========================================

Console.WriteLine("=== EXPECTED ===");
Console.WriteLine();

Console.WriteLine("1. Ashlin plays Normal Wild");
Console.WriteLine("Current Player AFTER PLAY: Rahul");
Console.WriteLine("Active Color AFTER PLAY:");
Console.WriteLine("Pending Draw AFTER PLAY: 0");

Console.WriteLine();

Console.WriteLine("2. Rahul tries to choose Wild");
Console.WriteLine("Success: False");
Console.WriteLine(
    "Message: Wild color must be Red, Blue, Green, or Yellow.");
Console.WriteLine("Active Color:");
Console.WriteLine("Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("3. Rahul chooses Blue");
Console.WriteLine("Success: True");
Console.WriteLine("Message: Wild color changed to Blue.");
Console.WriteLine("Active Color: Blue");
Console.WriteLine("Current Player: Rahul");
Console.WriteLine("Pending Draw: 0");