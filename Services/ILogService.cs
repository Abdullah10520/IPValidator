using IPValidatorAssignment.Models;

namespace IPValidatorAssignment.Services
{
    public interface ILogService
    {
        IEnumerable<BlockedAttemptLog> GetAllLogs();
        void LogAttempt(BlockedAttemptLog log);

        //bool AddTemporalBlock(string code, int minutes);
        //void RemoveExpiredBlocks();
    }
}
