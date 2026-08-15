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
        new Player { Name = "Arun" },
        new Player { Name = "Neha" }
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
// ASHLIN
// WILD DRAW SIX
// ========================================

var ashlinSix = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawSix
};

game.Players[0].Hand.Add(ashlinSix);

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 2
});

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
});

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 3
});

game.Players[0].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// RAHUL
// WILD DRAW TEN
// ========================================

var rahulTen = new Card
{
    Color = CardColor.Wild,
    Type = CardType.WildDrawTen
};

game.Players[1].Hand.Add(rahulTen);

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 2
});

game.Players[1].Hand.Add(new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 4
});

// ========================================
// ARUN
//
// These cards will rotate to Rahul
// after Ashlin plays 0.
// ========================================

var arunSkip = new Card
{
    Color = CardColor.Red,
    Type = CardType.Skip
};

var arunDiscardAll = new Card
{
    Color = CardColor.Red,
    Type = CardType.DiscardAll
};

game.Players[2].Hand.Add(arunSkip);

game.Players[2].Hand.Add(arunDiscardAll);

game.Players[2].Hand.Add(new Card
{
    Color = CardColor.Green,
    Type = CardType.Number,
    Number = 3
});

// ========================================
// NEHA
// ========================================

var nehaSeven = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 7
};

var nehaZero = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 0
};

var nehaReverse = new Card
{
    Color = CardColor.Red,
    Type = CardType.Reverse
};

var nehaExtra1 = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 8
};

var nehaExtra2 = new Card
{
    Color = CardColor.Red,
    Type = CardType.Number,
    Number = 9
};

game.Players[3].Hand.Add(nehaSeven);
game.Players[3].Hand.Add(nehaZero);
game.Players[3].Hand.Add(nehaReverse);
game.Players[3].Hand.Add(nehaExtra1);
game.Players[3].Hand.Add(nehaExtra2);

// ========================================
// DRAW DECK
//
// Arun will draw 16 cards.
// ========================================

for (int i = 0; i < 20; i++)
{
    game.Deck.Add(new Card
    {
        Color = CardColor.Green,
        Type = CardType.Number,
        Number = i % 10
    });
}

// ========================================
// HELPER
// ========================================

void PrintHands(Game game)
{
    foreach (var player in game.Players)
    {
        Console.WriteLine(
            $"{player.Name} ({player.Hand.Count} cards):");

        foreach (var card in player.Hand)
        {
            Console.WriteLine($"  - {card}");
        }

        Console.WriteLine();
    }
}

// ========================================
// INITIAL STATE
// ========================================

Console.WriteLine("========================================");
Console.WriteLine("=== 4 PLAYER FULL INTEGRATION TEST ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine("=== INITIAL STATE ===");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine();

PrintHands(game);

// ========================================
// STEP 1
// ASHLIN PLAYS WILD DRAW SIX
// ========================================

var result1 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    ashlinSix.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 1: ASHLIN PLAYS WILD DRAW SIX ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result1.Success}");

Console.WriteLine(
    $"Message: {result1.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

// ========================================
// STEP 2
// RAHUL PLAYS WILD DRAW TEN
// ========================================

var result2 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    rahulTen.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 2: RAHUL PLAYS WILD DRAW TEN ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result2.Success}");

Console.WriteLine(
    $"Message: {result2.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

// ========================================
// STEP 3
// ARUN TAKES +16
// ========================================

var result3 = gameService.TakeTurn(game);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 3: ARUN TAKES +16 ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"TakeTurn Result: {result3}");

Console.WriteLine(
    $"Arun Hand Count: {game.Players[2].Hand.Count}");

Console.WriteLine(
    $"Arun Eliminated: {game.Players[2].IsEliminated}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine();

// ========================================
// STEP 4
// NEHA PLAYS 7
// TARGET = ASHLIN
// ========================================

var result4 = gameService.PlayPlayerCard(
    game,
    "Neha",
    nehaSeven.Id,
    "Ashlin");

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 4: NEHA PLAYS RED 7 ===");
Console.WriteLine("=== TARGET: ASHLIN ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result4.Success}");

Console.WriteLine(
    $"Message: {result4.Message}");

Console.WriteLine();

Console.WriteLine("Hands AFTER 7 SWAP:");

PrintHands(game);

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine();

// ========================================
// STEP 5
// ASHLIN PLAYS RED 0
// ========================================

var result5 = gameService.PlayPlayerCard(
    game,
    "Ashlin",
    nehaZero.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 5: ASHLIN PLAYS RED 0 ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result5.Success}");

Console.WriteLine(
    $"Message: {result5.Message}");

Console.WriteLine();

Console.WriteLine("Hands AFTER 0 ROTATION:");

PrintHands(game);

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine();

// ========================================
// STEP 6
// RAHUL PLAYS RED SKIP
// ========================================

var skipCard = game.Players[1].Hand
    .First(c =>
        c.Color == CardColor.Red &&
        c.Type == CardType.Skip);

var result6 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    skipCard.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 6: RAHUL PLAYS RED SKIP ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result6.Success}");

Console.WriteLine(
    $"Message: {result6.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Turn Advance: {game.TurnAdvance}");

Console.WriteLine();

// ========================================
// STEP 7
// NEHA PLAYS RED REVERSE
// ========================================

var reverseCard = game.Players[3].Hand
    .First(c =>
        c.Color == CardColor.Red &&
        c.Type == CardType.Reverse);

var result7 = gameService.PlayPlayerCard(
    game,
    "Neha",
    reverseCard.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 7: NEHA PLAYS RED REVERSE ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result7.Success}");

Console.WriteLine(
    $"Message: {result7.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine();

// ========================================
// STEP 8
// ARUN PLAYS RED 2
//
// After Neha reverses:
// Direction = -1
// Current Player = Arun
// ========================================

var arunRedTwo = game.Players[2].Hand
    .First(c =>
        c.Color == CardColor.Red &&
        c.Type == CardType.Number &&
        c.Number == 2);

var result8 = gameService.PlayPlayerCard(
    game,
    "Arun",
    arunRedTwo.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 8: ARUN PLAYS RED 2 ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result8.Success}");

Console.WriteLine(
    $"Message: {result8.Message}");

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Current Player Index: {game.CurrentPlayerIndex}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();


// ========================================
// STEP 9
// RAHUL PLAYS RED DISCARD ALL
//
// Direction = -1
// Therefore Rahul is next.
// ========================================

var discardAllCard = game.Players[1].Hand
    .First(c =>
        c.Color == CardColor.Red &&
        c.Type == CardType.DiscardAll);

var result9 = gameService.PlayPlayerCard(
    game,
    "Rahul",
    discardAllCard.Id);

Console.WriteLine("========================================");
Console.WriteLine("=== STEP 9: RAHUL PLAYS RED DISCARD ALL ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine(
    $"Success: {result9.Success}");

Console.WriteLine(
    $"Message: {result9.Message}");

Console.WriteLine();

Console.WriteLine("Rahul Hand AFTER Discard All:");

foreach (var card in game.Players[1].Hand)
{
    Console.WriteLine($"- {card}");
}

Console.WriteLine();

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Current Player Index: {game.CurrentPlayerIndex}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine();

// ========================================
// FINAL STATE
// ========================================

Console.WriteLine("========================================");
Console.WriteLine("=== FINAL STATE ===");
Console.WriteLine("========================================");
Console.WriteLine();

PrintHands(game);

Console.WriteLine(
    $"Current Player: {gameService.GetCurrentPlayer(game).Name}");

Console.WriteLine(
    $"Current Player Index: {game.CurrentPlayerIndex}");

Console.WriteLine(
    $"Direction: {game.Direction}");

Console.WriteLine(
    $"Pending Draw: {game.PendingDrawCount}");

Console.WriteLine(
    $"Stack Value: {game.CurrentStackValue}");

Console.WriteLine(
    $"Eliminations: {game.Eliminations}");

Console.WriteLine();

// ========================================
// EXPECTED
// ========================================

Console.WriteLine("========================================");
Console.WriteLine("=== EXPECTED INTEGRATION FLOW ===");
Console.WriteLine("========================================");
Console.WriteLine();

Console.WriteLine("1. Ashlin -> Wild Draw Six");
Console.WriteLine("   Pending Draw: 6");
Console.WriteLine("   Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("2. Rahul -> Wild Draw Ten");
Console.WriteLine("   Pending Draw: 16");
Console.WriteLine("   Stack Value: 10");
Console.WriteLine("   Current Player: Arun");

Console.WriteLine();

Console.WriteLine("3. Arun -> takes 16 cards");
Console.WriteLine("   Mercy Rule NOT triggered");
Console.WriteLine("   Current Player: Neha");

Console.WriteLine();

Console.WriteLine("4. Neha -> Red 7 -> chooses Ashlin");
Console.WriteLine("   Current Player: Ashlin");

Console.WriteLine();

Console.WriteLine("5. Ashlin -> Red 0");
Console.WriteLine("   Hands rotate");
Console.WriteLine("   Current Player: Rahul");

Console.WriteLine();

Console.WriteLine("6. Rahul -> Red Skip");
Console.WriteLine("   Arun is skipped");
Console.WriteLine("   Current Player: Neha");

Console.WriteLine();

Console.WriteLine("7. Neha -> Red Reverse");
Console.WriteLine("   Direction: -1");
Console.WriteLine("   Current Player: Arun");

Console.WriteLine();

Console.WriteLine("8. Arun -> Red Discard All");
Console.WriteLine("   Red cards are discarded");
Console.WriteLine("   Pending Draw: 0");
Console.WriteLine("   Stack Value: 0");

Console.WriteLine();

Console.WriteLine("=== INTEGRATION TEST COMPLETE ===");