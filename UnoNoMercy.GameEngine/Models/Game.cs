namespace UnoNoMercy.GameEngine.Models
{
    public class Game
    {
        public List<Player> Players { get; set; } = new();

        public List<Card> Deck { get; set; } = new();

        public List<Card> DiscardPile { get; set; } = new();

        public int CurrentPlayerIndex { get; set; } = 0;

        public int Direction { get; set; } = 1;
    }
}
