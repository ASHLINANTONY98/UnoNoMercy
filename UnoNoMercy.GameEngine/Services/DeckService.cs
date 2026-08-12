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
                for (int i = 0; i <= 9; i++)
                {
                    deck.Add(new Card
                    {
                        Color = color,
                        Type = CardType.Number,
                        Number = i
                    });
                }

                // Skip Cards
                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.Skip
                });

                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.Skip
                });

                // Reverse Cards
                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.Reverse
                });

                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.Reverse
                });

                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.DrawTwo
                });

                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.DrawTwo
                });

                deck.Add(new Card
                {
                    Color = color,
                    Type = CardType.DrawTen
                });
            }

            for (int i = 0; i < 4; i++)
            {
                deck.Add(new Card
                {
                    Color = CardColor.Wild,
                    Type = CardType.Wild
                });
            }

            for (int i = 0; i < 4; i++)
            {
                deck.Add(new Card
                {
                    Color = CardColor.Wild,
                    Type = CardType.WildDrawFour
                });
            }

            for (int i = 0; i < 4; i++)
            {
                deck.Add(new Card
                {
                    Color = CardColor.Wild,
                    Type = CardType.WildDrawSix
                });
            }

            return deck;
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
