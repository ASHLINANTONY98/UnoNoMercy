using Microsoft.AspNetCore.Mvc;
using UnoNoMercy.Api.Models;
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
    private readonly GameService _gameService;
    private readonly DeckService _deckService;

    public GameController(GameManager gameManager, GameService gameService, DeckService deckService)
    {
        _gameManager = gameManager;
        _gameService = gameService;
        _deckService = deckService;

    }

    [HttpPost("create")]
    public IActionResult CreateGame(
    CreateGameRequest request)
    {
        if (request.Players.Count < 2)
        {
            return BadRequest(
                "At least 2 players are required.");
        }

        var deck = _deckService.CreateDeck();

        _deckService.Shuffle(deck);

        var game = new Game
        {
            Deck = deck
        };

        foreach (var playerName in request.Players)
        {
            game.Players.Add(
                new Player
                {
                    Name = playerName
                });
        }


        _gameService.DealCards(game);

        Card startingCard;

        while (true)
        {
            startingCard = _gameService.DrawCard(game);

            if (startingCard.Type == CardType.Number)
            {
                game.DiscardPile.Add(startingCard);
                break;
            }

            game.Deck.Add(startingCard);

            _deckService.Shuffle(game.Deck);
        }

        _gameManager.CurrentGame = game;

        return Ok(
            _gameService.GetGameState(game));
    }

    [HttpGet("state")]
    public IActionResult GetState()
    {
        if (_gameManager.CurrentGame == null)
        {
            return NotFound(
                "No active game found.");
        }

        return Ok(
            _gameService.GetGameState(
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

        var continueGame =
            _gameService.TakeTurn(
                _gameManager.CurrentGame);

        return Ok(
            _gameService.GetGameState(
                _gameManager.CurrentGame));
    }

}