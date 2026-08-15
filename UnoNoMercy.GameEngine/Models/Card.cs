using UnoNoMercy.GameEngine.Enums;

namespace UnoNoMercy.GameEngine.Models
{
    public class Card
    {
        public string Id { get; set; }
            = Guid.NewGuid().ToString();

        public CardColor Color { get; set; }

        public CardType Type { get; set; }

        public int? Number { get; set; }

        public override string ToString()
        {
            if (Type == CardType.Number)
                return $"{Color} {Number}";

            return Type switch
            {
                CardType.Wild =>
                    "Wild",

                CardType.WildReverseDrawFour =>
                    "Wild Reverse Draw Four",

                CardType.WildDrawSix =>
                    "Wild Draw Six",

                CardType.WildDrawTen =>
                    "Wild Draw Ten",

                CardType.WildColorRoulette =>
                    "Wild Color Roulette",

                _ =>
                    $"{Color} {Type}"
            };
        }
    }
}