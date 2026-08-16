namespace UnoNoMercy.GameEngine.DTOs
{
    public class PlayCardRequest
    {
        public string SessionToken { get; set; }
            = string.Empty;

        public string CardId { get; set; }
            = string.Empty;
    }
}