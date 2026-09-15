namespace UnoNoMercy.Api.Tests.Helpers;

public class RoomJoinedMessage
{
    public string RoomCode { get; set; }
        = string.Empty;

    public string PlayerName { get; set; }
        = string.Empty;

    public string SessionToken { get; set; }
        = string.Empty;
}