using UnoNoMercy.GameEngine.Enums;

namespace UnoNoMercy.GameEngine.Models
{
    public class Game
    {
        public List<Player> Players { get; set; } = new();

        public List<Card> Deck { get; set; } = new();

        public List<Card> DiscardPile { get; set; } = new();

        public int CurrentPlayerIndex { get; set; } = 0;

        public int Direction { get; set; } = 1;

        public CardColor? ActiveColor { get; set; }

        public int PendingDrawCount { get; set; } = 0;

        public int CurrentStackValue { get; set; } = 0;
    }
}
