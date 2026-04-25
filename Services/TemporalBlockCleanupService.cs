namespace IPValidatorAssignment.Services
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly IBlockedCountryService _blockedService;
        private readonly ILogService _log;
        private readonly TimeSpan _checkInterval = TimeSpan.FromMinutes(5);

        public TemporalBlockCleanupService(IBlockedCountryService blockedService, ILogService log)
        {
            _blockedService = blockedService;
            _log = log;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                // تنفيذ عملية التنظيف
                _blockedService.RemoveExpiredBlocks();

                // الانتظار لمدة 5 دقائق
                await Task.Delay(_checkInterval, stoppingToken);
            }
        }
    }
}
