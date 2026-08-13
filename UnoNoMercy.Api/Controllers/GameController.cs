using Microsoft.AspNetCore.Mvc;
using UnoNoMercy.Api.Models;
using UnoNoMercy.Api.Services;
using UnoNoMercy.GameEngine.DTOs;
using UnoNoMercy.GameEngine.DTOs.UnoNoMercy.GameEngine.DTOs;
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

        var gameId = Guid.NewGuid();

        game.RoomCode =
            RoomCodeGenerator.Generate();

        _gameManager.Games[gameId] = game;

        return Ok(
            new CreateGameResponse
            {
                GameId = gameId,
                RoomCode = game.RoomCode,
                State = _gameService.GetGameState(game)
            });

    }

    [HttpGet("state/{gameId}")]
    public IActionResult GetState(Guid gameId)
    {
        if (!_gameManager.Games.TryGetValue(
            gameId,
            out var game))
        {
            return NotFound("Game not found.");
        }

        return Ok(
            _gameService.GetGameState(game));
    }

    [HttpPost("next-turn/{gameId}")]
    public IActionResult NextTurn(Guid gameId)
    {
        if (!_gameManager.Games.TryGetValue(
            gameId,
            out var game))
        {
            return NotFound("Game not found.");
        }

        if (game.IsGameOver)
        {
            return BadRequest(
                "Game is already finished.");
        }

        _gameService.TakeTurn(game);

        return Ok(
            _gameService.GetGameState(game));
    }

    [HttpGet("result/{gameId}")]
    public IActionResult GetResult(Guid gameId)
    {
        if (!_gameManager.Games.TryGetValue(
            gameId,
            out var game))
        {
            return NotFound(
                "Game not found.");
        }

        return Ok(
            new GameResultDto
            {
                IsGameOver = game.IsGameOver,
                WinnerName = game.WinnerName,
                TotalTurns = game.TotalTurns,
                LargestStack = game.LargestStack,
                Eliminations = game.Eliminations
            });
    }

    [HttpGet("hand/{gameId}/{playerName}")]
    public IActionResult GetHand(
        Guid gameId,
        string playerName)
    {
        if (!_gameManager.Games.TryGetValue(
            gameId,
            out var game))
        {
            return NotFound(
                "Game not found.");
        }

        return Ok(
            _gameService.GetPlayerHand(
                game,
                playerName));
    }

    [HttpPost("play-card")]
    public IActionResult PlayCard(
    PlayCardRequest request)
    {
        if (!_gameManager.Games.TryGetValue(
            request.GameId,
            out var game))
        {
            return NotFound(
                "Game not found.");
        }

        var result =
            _gameService.PlayPlayerCard(
                game,
                request.PlayerName,
                request.CardId);

        return Ok(result);
    }
    [HttpPost("draw-card")]
    public IActionResult DrawCard(
        DrawCardRequest request)
    {
        if (!_gameManager.Games.TryGetValue(
            request.GameId,
            out var game))
        {
            return NotFound(
                "Game not found.");
        }

        var result =
            _gameService.DrawPlayerCard(
                game,
                request.PlayerName);

        return Ok(result);
    }

    [HttpPost("pass-turn")]
    public IActionResult PassTurn(
        PassTurnRequest request)
    {
        if (!_gameManager.Games.TryGetValue(
            request.GameId,
            out var game))
        {
            return NotFound(
                "Game not found.");
        }

        var result =
            _gameService.PassTurn(
                game,
                request.PlayerName);

        if (!result)
        {
            return BadRequest(
                "Unable to pass turn.");
        }

        return Ok(
            _gameService.GetGameState(game));
    }

    [HttpPost("join")]
    public IActionResult JoinGame(
    JoinGameRequest request)
    {
        var gameEntry = _gameManager.Games
            .FirstOrDefault(x =>
                x.Value.RoomCode ==
                request.RoomCode);

        if (gameEntry.Value == null)
        {
            return NotFound(
                "Room not found.");
        }

        var game = gameEntry.Value;

        if (game.Players.Any(x =>
            x.Name == request.PlayerName))
        {
            return BadRequest(
                "Player already exists.");
        }

        game.Players.Add(
            new Player
            {
                Name = request.PlayerName
            });

        return Ok(
            _gameService.GetGameState(game));
    }

}