namespace UnoNoMercy.Api.Services
{
    public static class RoomCodeGenerator
    {
        private const string Chars =
            "ABCDEFGHIJKLMNOPQRSTUVWXYZ123456789";

        public static string Generate()
        {
            var random = new Random();

            return new string(
                Enumerable.Repeat(
                    Chars,
                    6)
                .Select(x =>
                    x[random.Next(x.Length)])
                .ToArray());
        }
    }
}