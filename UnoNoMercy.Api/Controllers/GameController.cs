using Microsoft.AspNetCore.Mvc;
using UnoNoMercy.Api.Services;
using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;
using UnoNoMercy.GameEngine.Services;

namespace UnoNoMercy.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class GameController : ControllerBase
{
    private readonly GameManager _gameManager;

    public GameController(GameManager gameManager)
    {
        _gameManager = gameManager;
    }

    [HttpPost("create")]
    public IActionResult CreateGame()
    {
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

        Card startingCard;

        while (true)
        {
            startingCard = gameService.DrawCard(game);

            if (startingCard.Type == CardType.Number)
            {
                game.DiscardPile.Add(startingCard);
                break;
            }

            game.Deck.Add(startingCard);

            deckService.Shuffle(game.Deck);
        }

        _gameManager.CurrentGame = game;

        return Ok(
            gameService.GetGameState(game));
    }

    [HttpGet("state")]
    public IActionResult GetState()
    {
        if (_gameManager.CurrentGame == null)
        {
            return NotFound(
                "No active game found.");
        }

        var gameService = new GameService();

        return Ok(
            gameService.GetGameState(
                _gameManager.CurrentGame));
    }

    [HttpPost("next-turn")]
    public IActionResult NextTurn()
    {
        if (_gameManager.CurrentGame == null)
        {
            return NotFound(
                "No active game found.");
        }

        var gameService = new GameService();

        var continueGame =
            gameService.TakeTurn(
                _gameManager.CurrentGame);

        return Ok(
            gameService.GetGameState(
                _gameManager.CurrentGame));
    }

}