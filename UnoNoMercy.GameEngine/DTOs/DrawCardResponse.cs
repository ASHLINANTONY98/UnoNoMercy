namespace UnoNoMercy.GameEngine.DTOs
{
    public class DrawCardResponse
    {
        public bool Success { get; set; }

        public string Message { get; set; }

        public string? DrawnCard { get; set; }
    }
}
