using UnoNoMercy.GameEngine.Enums;
using UnoNoMercy.GameEngine.Models;

namespace UnoNoMercy.GameEngine.Services
{
    public class DeckService
    {
        public List<Card> CreateDeck()
        {
            var deck = new List<Card>();

            var colors = new[]
            {
                CardColor.Red,
                CardColor.Blue,
                CardColor.Green,
                CardColor.Yellow
            };

            foreach (var color in colors)
            {
                // Number cards
                // Two copies of 0-9 for each color
                for (int number = 0; number <= 9; number++)
                {
                    for (int copy = 0; copy < 2; copy++)
                    {
                        deck.Add(new Card
                        {
                            Color = color,
                            Type = CardType.Number,
                            Number = number
                        });
                    }
                }

                // Colored action cards
                AddCards(deck, color, CardType.Skip, 3);
                AddCards(deck, color, CardType.Reverse, 3);
                AddCards(deck, color, CardType.DrawTwo, 3);
                AddCards(deck, color, CardType.DrawFour, 2);
                AddCards(deck, color, CardType.DiscardAll, 3);
                AddCards(deck, color, CardType.SkipEveryone, 2);
            }

            // Wild cards
            AddWildCards(
                deck,
                CardType.WildReverseDrawFour,
                8);

            AddWildCards(
                deck,
                CardType.WildDrawSix,
                4);

            AddWildCards(
                deck,
                CardType.WildDrawTen,
                4);

            AddWildCards(
                deck,
                CardType.WildColorRoulette,
                8);

            return deck;
        }

        private void AddCards(
            List<Card> deck,
            CardColor color,
            CardType type,
            int count)
        {
            for (int i = 0; i < count; i++)
            {
                deck.Add(new Card
                {
                    Color = color,
                    Type = type
                });
            }
        }

        private void AddWildCards(
            List<Card> deck,
            CardType type,
            int count)
        {
            for (int i = 0; i < count; i++)
            {
                deck.Add(new Card
                {
                    Color = CardColor.Wild,
                    Type = type
                });
            }
        }

        public void Shuffle(List<Card> deck)
        {
            var rng = new Random();

            int n = deck.Count;

            while (n > 1)
            {
                n--;

                int k = rng.Next(n + 1);

                (deck[n], deck[k]) =
                    (deck[k], deck[n]);
            }
        }
    }
}