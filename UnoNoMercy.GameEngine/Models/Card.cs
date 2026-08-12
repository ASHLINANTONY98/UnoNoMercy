using UnoNoMercy.GameEngine.Enums;

namespace UnoNoMercy.GameEngine.Models
{
    public class Card
    {
        public CardColor Color { get; set; }
        public CardType Type { get; set; }
        public int? Number { get; set; }
        public override string ToString()
        {
            if (Type == CardType.Number)
                return $"{Color} {Number}";
 
            if (Type == CardType.WildDrawFour)
                return "Wild Draw Four";

            if (Type == CardType.Wild)
                return "Wild";

            if (Type == CardType.WildDrawSix)
                return "Wild Draw Six";

            if (Type == CardType.DrawTen)
                return $"{Color} Draw Ten";

            return $"{Color} {Type}";
        }
    }
}
