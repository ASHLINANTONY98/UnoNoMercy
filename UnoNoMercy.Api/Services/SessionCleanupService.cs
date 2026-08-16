namespace UnoNoMercy.Api.Services
{
    public class SessionCleanupService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;

        public SessionCleanupService(
            IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(
            CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    using var scope =
                        _serviceProvider.CreateScope();

                    var gameManager =
                        scope.ServiceProvider
                            .GetRequiredService<GameManager>();

                    gameManager.RemoveExpiredSessions(
                        TimeSpan.FromHours(24));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(
                        $"[SessionCleanup] {ex.Message}");
                }

                await Task.Delay(
                    TimeSpan.FromMinutes(10),
                    stoppingToken);
            }
        }
    }
}