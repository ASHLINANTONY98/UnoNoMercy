using UnoNoMercy.GameEngine.Models;
using UnoNoMercy.GameEngine.Services;

var deckService = new DeckService();

var deck = deckService.CreateDeck();

deckService.Shuffle(deck);

var game = new Game
{
    Deck = deck,
    Players =
    {
        new Player { Name = "Ashlin" },
        new Player { Name = "Rahul" },
        new Player { Name = "Arun" },
        new Player { Name = "Vishnu" }
    }
};

var gameService = new GameService();

gameService.DealCards(game);

var startingCard = gameService.DrawCard(game);

game.DiscardPile.Add(startingCard);

Console.WriteLine($"Starting Card: {startingCard}");
Console.WriteLine();

Console.WriteLine("=== GAME STARTED ===");
Console.WriteLine();

while (true)
{
    bool continueGame =
        gameService.TakeTurn(game);

    if (!continueGame)
        break;
}