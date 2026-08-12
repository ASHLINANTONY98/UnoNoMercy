namespace UnoNoMercy.GameEngine.Models
{
    public class Player
    {
        public string Name { get; set; } = string.Empty;

        public List<Card> Hand { get; set; } = new();

        public bool IsEliminated { get; set; } = false;
    }
}
