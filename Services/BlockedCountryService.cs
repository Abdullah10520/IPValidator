using IPValidatorAssignment.Models;
using IPValidatorAssignment.Repositories;
using System.Collections.Concurrent;
using static IPValidatorAssignment.Services.BlockedCountryService;
using static IPValidatorAssignment.Services.TestService;

namespace IPValidatorAssignment.Services
{
    
    public class BlockedCountryService : IBlockedCountryService
    {
        //private ConcurrentDictionary<string, byte> _blockedCountries = new();
        private readonly IBlockCountryRepository _repository;
        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        //private readonly ConcurrentBag<BlockedAttemptLog> _logs = new();
        //private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        public BlockedCountryService(IBlockCountryRepository repository)
        {
            _repository = repository;
        }   

        public bool AddCountry(string countryCode)
        {
            //return _blockedCountries.TryAdd(countryCode.ToUpper(), 0);
            return _repository.AddCountry(countryCode);
        }

        public bool RemoveCountry(string countryCode)
        {
            //return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
            return _repository.RemoveCountry(countryCode);
        }

        public IEnumerable<string> GetBlockedCountries(string? search, int pageNumber, int pageSize)
        {
            //var pagedBlocked = _blockedCountries.Keys.ToList().Skip((pageNumber - 1) * pageSize).Take(pageSize);
            var pagedBlocked = _repository.GetBlockedCountries(pageNumber, pageSize);

            if (!string.IsNullOrEmpty(search))
            {
                pagedBlocked = pagedBlocked.Where(c => c.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return pagedBlocked;
        }

        public bool IsCountryBlocked(string countryCode)
        {
            //if (_temporalBlocks.TryGetValue(countryCode, out var expiry))
            //{
            //    if (expiry > DateTime.UtcNow) return true;
            //}
            //return _blockedCountries.ContainsKey(countryCode.ToUpper());

            return _repository.IsCountryBlocked(countryCode);
        }


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

        //public void LogAttempt(BlockedAttemptLog log)
        //{
        //    _logs.Add(log);
        //}
        //public IEnumerable<BlockedAttemptLog> GetAllLogs()
        //{
        //    return _logs.ToList();
        //}




        //public class TemporalBlock
        //{
        //    public string CountryCode { get; set; }
        //    public DateTime ExpirationTime { get; set; }
        //}

        // جوه الـ BlockedCountryService

        //public bool AddTemporalBlock(string code, int minutes)
        //{
        //    var expiry = DateTime.UtcNow.AddMinutes(minutes);
        //    // TryAdd بترجع false لو الكود موجود فعلاً (عشان نحقق شرط الـ Conflict)
        //    return _temporalBlocks.TryAdd(code.ToUpper(), expiry);
        //}

        //public void RemoveExpiredBlocks()
        //{
        //    var now = DateTime.UtcNow;
        //    var expiredKeys = _temporalBlocks.Where(x => x.Value <= now).Select(x => x.Key);

        //    foreach (var key in expiredKeys)
        //    {
        //        _temporalBlocks.TryRemove(key, out _);
        //    }
        //}





    }
    
}
