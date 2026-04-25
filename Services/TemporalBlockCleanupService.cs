namespace IPValidatorAssignment.Services
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IBlockedCountryService _blockedService;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(IBlockedCountryService blockedService)
        {
            _blockedService = blockedService;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _blockedService.RemoveExpiredBlocks();

                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
