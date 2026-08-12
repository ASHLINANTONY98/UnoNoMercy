namespace UnoNoMercy.Api.Models
{
    public class CreateGameRequest
    {
        public List<string> Players { get; set; }
            = new();
    }
}