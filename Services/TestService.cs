using System.Collections.Concurrent;

namespace IPValidatorAssignment.Services
{
    public class TestService
    {
        public class TemporalBlock
        {
            public string CountryCode { get; set; }
            public DateTime ExpirationTime { get; set; }
        }

        // جوه الـ BlockedCountryService
        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        public bool AddTemporalBlock(string code, int minutes)
        {
            var expiry = DateTime.UtcNow.AddMinutes(minutes);
            // TryAdd بترجع false لو الكود موجود فعلاً (عشان نحقق شرط الـ Conflict)
            return _temporalBlocks.TryAdd(code.ToUpper(), expiry);
        }

        public void RemoveExpiredBlocks()
        {
            var now = DateTime.UtcNow;
            var expiredKeys = _temporalBlocks.Where(x => x.Value <= now).Select(x => x.Key);

            foreach (var key in expiredKeys)
            {
                _temporalBlocks.TryRemove(key, out _);
            }
        }
    }
}
