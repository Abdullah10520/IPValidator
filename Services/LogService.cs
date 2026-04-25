using IPValidatorAssignment.Models;
using System.Collections.Concurrent;
using static IPValidatorAssignment.Services.TestService;
using static System.Reflection.Metadata.BlobBuilder;

namespace IPValidatorAssignment.Services
{
    public class LogService : ILogService
    {
        private readonly ConcurrentBag<BlockedAttemptLog> _logs = new();
        public void LogAttempt(BlockedAttemptLog log)
        {
            _logs.Add(log);
        }
        public IEnumerable<BlockedAttemptLog> GetAllLogs()
        {
            return _logs.ToList();
        }
    }
}
