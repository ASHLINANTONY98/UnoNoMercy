namespace UnoNoMercy.GameEngine.DTOs
{
    public class GameResultDto
    {
        public bool IsGameOver { get; set; }

        public string? WinnerName { get; set; }

        public int TotalTurns { get; set; }

        public int LargestStack { get; set; }

        public int Eliminations { get; set; }
    }
}
