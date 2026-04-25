using IPValidatorAssignment.Models;

namespace IPValidatorAssignment.Services
{
    public interface ILogService
    {
        IEnumerable<BlockedAttemptLog> GetAllLogs();
        void LogAttempt(BlockedAttemptLog log);


    }
}
