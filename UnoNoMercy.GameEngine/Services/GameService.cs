using System.Numerics;
using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.GameEngine.Services
{
    public class GameService
    {
        public Card DrawCard(Game game)
        {
            if (game.Deck.Count == 0)
            {
                RebuildDeck(game);
            }

            var card = game.Deck[0];

            game.Deck.RemoveAt(0);

            return card;
        }
        private void RebuildDeck(Game game)
        {
            if (game.DiscardPile.Count <= 1)
                throw new Exception("No cards available.");

            var topCard = game.DiscardPile.Last();

            var cardsToShuffle =
                game.DiscardPile
                    .Take(game.DiscardPile.Count - 1)
                    .ToList();

            game.DiscardPile.Clear();

            game.DiscardPile.Add(topCard);

            var rng = new Random();

            game.Deck = cardsToShuffle
                .OrderBy(x => rng.Next())
                .ToList();

            Console.WriteLine(
                "🔄 Deck rebuilt from discard pile.");
        }

        public void DealCards(Game game, int cardsPerPlayer = 7)
        {
            foreach (var player in game.Players)
            {
                for (int i = 0; i < cardsPerPlayer; i++)
                {
                    player.Hand.Add(DrawCard(game));
                }
            }
        }

        public Player GetCurrentPlayer(Game game)
        {
            while (game.Players[game.CurrentPlayerIndex]
                .IsEliminated)
            {
                NextTurn(game);
            }

            return game.Players[
                game.CurrentPlayerIndex];
        }

        public void NextTurn(Game game)
        {
            game.CurrentPlayerIndex += game.Direction;

            if (game.CurrentPlayerIndex >= game.Players.Count)
            {
                game.CurrentPlayerIndex = 0;
            }

            if (game.CurrentPlayerIndex < 0)
            {
                game.CurrentPlayerIndex =
                    game.Players.Count - 1;
            }
        }

        public bool PlayCard(
            Game game,
            Player player,
            Card card)
        {
            if (!player.Hand.Contains(card))
                return false;

            var topCard = GetTopCard(game);

            if (!CanPlayCard(game, card, topCard))
                return false;

            if (!IsWildCard(card))
            {
                game.ActiveColor = null;
            }

            player.Hand.Remove(card);

            game.DiscardPile.Add(card);

            return true;
        }

        private bool IsWildCard(Card card)
        {
            return card.Type.ToString().StartsWith("Wild");
        }


        public bool CanPlayCard(
            Game game,
            Card cardToPlay,
            Card topCard)
        {
            if (cardToPlay.Type == CardType.Wild ||
                cardToPlay.Type == CardType.WildDrawFour ||
                cardToPlay.Type == CardType.WildDrawSix)
            {
                return true;
            }


            if (game.ActiveColor != null)
            {
                if (cardToPlay.Color ==
                    game.ActiveColor)
                    return true;
            }

            if (cardToPlay.Color ==
                topCard.Color)
                return true;

            if (cardToPlay.Type ==
                topCard.Type)
                return true;

            if (cardToPlay.Number ==
                topCard.Number)
                return true;

            return false;
        }

        public Card GetTopCard(Game game)
        {
            return game.DiscardPile.Last();
        }

        public Card? GetFirstPlayableCard(
            Game game,
            Player player,
            Card topCard)
        {
            return player.Hand
                .FirstOrDefault(card =>
                    CanPlayCard(
                        game,
                        card,
                        topCard));
        }

        public bool TakeTurn(Game game)
        {
            if (IsLastPlayerStanding(game))
            {
                var winner = game.Players
                    .First(p => !p.IsEliminated);

                Console.WriteLine(
                    $"👑 {winner.Name} is the last player standing!");

                return false;
            }

            var player = GetCurrentPlayer(game);

            // NO MERCY STACKING CHECK
            if (game.PendingDrawCount > 0)
            {
                var stackCard = player.Hand
                    .Where(c => GetDrawValue(c) > 0)
                    .OrderBy(c => GetDrawValue(c))
                    .FirstOrDefault(c =>
                        GetDrawValue(c) >=
                        game.CurrentStackValue);

                if (stackCard != null)
                {
                    PlayCard(
                        game,
                        player,
                        stackCard);

                    Console.WriteLine(
                        $"{player.Name} stacked {stackCard}");

                    ProcessSpecialCard(
                        game,
                        player,
                        stackCard);

                    NextTurn(game);

                    return true;
                }

                for (int i = 0; i < game.PendingDrawCount; i++)
                {
                    player.Hand.Add(
                        DrawCard(game));
                }

                CheckMercyRule(player);

                if (player.IsEliminated)
                {
                    
                    game.PendingDrawCount = 0;
                    game.CurrentStackValue = 0;

                    NextTurn(game);

                    return true;
                }

                Console.WriteLine(
                    $"💀 {player.Name} draws {game.PendingDrawCount} cards!");

                game.PendingDrawCount = 0;
                game.CurrentStackValue = 0;

                NextTurn(game);

                return true;
            }

            var topCard = GetTopCard(game);

            var playableCard =
                GetFirstPlayableCard(
                    game,
                    player,
                    topCard);

            if (playableCard != null)
            {
                PlayCard(
                    game,
                    player,
                    playableCard);

                Console.WriteLine(
                    $"{player.Name} played {playableCard}");

                ProcessSpecialCard(
                    game,
                    player,
                    playableCard);
            }
            else
            {
                var drawnCard = DrawCard(game);

                player.Hand.Add(drawnCard);

                CheckMercyRule(player);

                Console.WriteLine(
                    $"{player.Name} drew {drawnCard}");
            }

            if (HasWon(player))
            {
                Console.WriteLine(
                    $"🏆 {player.Name} WINS!");

                return false;
            }

            NextTurn(game);

            return true;
        }

        public bool HasWon(Player player)
        {
            return player.Hand.Count == 0;
        }

        private void ProcessSpecialCard(
            Game game,
            Player player,
            Card card)
        {
            if (card.Type == CardType.Number)
            {
                if (card.Number == 7)
                {
                    var targetPlayer =
                        GetPlayerWithMostCards(game);

                    if (targetPlayer != player)
                    {
                        var tempHand = player.Hand;

                        player.Hand = targetPlayer.Hand;

                        targetPlayer.Hand = tempHand;

                        Console.WriteLine(
                            $"🔄 {player.Name} swapped hands with {targetPlayer.Name}!");
                    }
                }

                if (card.Number == 0)
                {
                    RotateHands(game);

                    Console.WriteLine(
                        "♻ Hands rotated!");
                }
            }
            switch (card.Type)
            {
                case CardType.Skip:
                    {
                        NextTurn(game);

                        Console.WriteLine(
                            "⏭ Skip card activated!");

                        break;
                    }

                case CardType.Reverse:
                    {
                        game.Direction *= -1;

                        Console.WriteLine(
                            "🔄 Reverse activated!");

                        break;
                    }

                case CardType.DrawTwo:
                    {
                        game.PendingDrawCount += 2;
                        game.CurrentStackValue = 2;

                        Console.WriteLine(
                            $"🔥 Draw penalty = {game.PendingDrawCount}");

                        break;
                    }

                case CardType.Wild:
                    {

                        game.ActiveColor = GetBestColor(player);

                        Console.WriteLine(
                            $"🌈 Wild! Color changed to {game.ActiveColor}");

                        break;
                    }

                case CardType.WildDrawFour:
                    {
                        game.ActiveColor =
                            GetBestColor(player);

                        game.PendingDrawCount += 4;
                        game.CurrentStackValue = 4;

                        Console.WriteLine(
                            $"🔥 Draw penalty = {game.PendingDrawCount}");

                        Console.WriteLine(
                            $"🌈 Wild Draw Four! Color changed to {game.ActiveColor}");

                        break;
                    }

                case CardType.WildDrawSix:
                    {
                        game.ActiveColor =
                            GetBestColor(player);

                        game.PendingDrawCount += 6;
                        game.CurrentStackValue = 6;

                        Console.WriteLine(
                            $"🔥 Draw penalty = {game.PendingDrawCount}");

                        Console.WriteLine(
                            $"🌈 Wild Draw Six! Color changed to {game.ActiveColor}");

                        break;
                    }

                case CardType.DrawTen:
                    {
                        game.PendingDrawCount += 10;
                        game.CurrentStackValue = 10;

                        Console.WriteLine(
                            $"💀 Draw penalty = {game.PendingDrawCount}");

                        break;
                    }
            }
        }

        private CardColor GetBestColor(Player player)
        {
            var colors = player.Hand
                .Where(c => c.Color != CardColor.Wild)
                .GroupBy(c => c.Color)
                .OrderByDescending(g => g.Count())
                .FirstOrDefault();

            return colors?.Key ?? CardColor.Red;
        }

        private Player GetPlayerWithMostCards(Game game)
        {
            return game.Players
                .OrderByDescending(p => p.Hand.Count)
                .First();
        }

        private void RotateHands(Game game)
        {
            var lastHand =
                game.Players.Last().Hand;

            for (int i = game.Players.Count - 1; i > 0; i--)
            {
                game.Players[i].Hand =
                    game.Players[i - 1].Hand;
            }

            game.Players[0].Hand = lastHand;
        }

        private void CheckMercyRule(Player player)
        {
            if (player.Hand.Count >= 25)
            {
                player.IsEliminated = true;

                player.Hand.Clear();

                Console.WriteLine(
                    $"☠ {player.Name} has been eliminated!");
            }
        }

        private bool IsLastPlayerStanding(Game game)
        {
            return game.Players
                .Count(p => !p.IsEliminated) == 1;
        }

        private int GetDrawValue(Card card)
        {
            return card.Type switch
            {
                CardType.DrawTwo => 2,
                CardType.WildDrawFour => 4,
                CardType.WildDrawSix => 6,
                CardType.DrawTen => 10,
                _ => 0
            };
        }
    }
}
