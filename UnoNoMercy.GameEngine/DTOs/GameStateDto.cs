namespace UnoNoMercy.GameEngine.DTOs
{
    public class GameStateDto
    {
        public string CurrentPlayer { get; set; }

        public string TopCard { get; set; }

        public int PendingDrawCount { get; set; }

        public int Direction { get; set; }

        public List<PlayerStateDto> Players { get; set; }
            = new();
    }
}
