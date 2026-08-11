using System.Numerics;
using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.GameEngine.Services
{
    public class GameService
    {
        public Card DrawCard(Game game)
        {
            var card = game.Deck[0];

            game.Deck.RemoveAt(0);

            return card;
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
            return game.Players[game.CurrentPlayerIndex];
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

            if (card.Color != CardColor.Wild)
            {
                game.ActiveColor = null;
            }

            player.Hand.Remove(card);

            game.DiscardPile.Add(card);

            return true;
        }

        public bool CanPlayCard(
            Game game,
            Card cardToPlay,
            Card topCard)
        {
            if (cardToPlay.Type == CardType.Wild ||
                cardToPlay.Type == CardType.WildDrawFour)
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
            var player = GetCurrentPlayer(game);

            var topCard = GetTopCard(game);

            var playableCard =
                GetFirstPlayableCard(game, player, topCard);

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
                        NextTurn(game);

                        var targetPlayer =
                            GetCurrentPlayer(game);

                        targetPlayer.Hand.Add(
                            DrawCard(game));

                        targetPlayer.Hand.Add(
                            DrawCard(game));

                        Console.WriteLine(
                            $"➕2 {targetPlayer.Name} draws 2 cards!");

                        NextTurn(game);

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

                        Console.WriteLine(
                            $"🌈 Wild Draw Four! Color changed to {game.ActiveColor}");

                        NextTurn(game);

                        var targetPlayer =
                            GetCurrentPlayer(game);

                        for (int i = 0; i < 4; i++)
                        {
                            targetPlayer.Hand.Add(
                                DrawCard(game));
                        }

                        Console.WriteLine(
                            $"➕4 {targetPlayer.Name} draws 4 cards!");

                        NextTurn(game);

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
    }
}
