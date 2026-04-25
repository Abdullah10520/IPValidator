using IPValidatorAssignment.Models;
using IPValidatorAssignment.Repositories;
using System.Collections.Concurrent;
using static IPValidatorAssignment.Services.BlockedCountryService;

namespace IPValidatorAssignment.Services
{
    
    public class BlockedCountryService : IBlockedCountryService
    {
        private readonly IBlockCountryRepository _repository;
        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        public BlockedCountryService(IBlockCountryRepository repository)
        {
            _repository = repository;
        }   

        public bool AddCountry(string countryCode)
        {
            return _repository.AddCountry(countryCode);
        }

        public bool RemoveCountry(string countryCode)
        {
            return _repository.RemoveCountry(countryCode);
        }

        public IEnumerable<string> GetBlockedCountries(string? search, int pageNumber, int pageSize)
        {
            var pagedBlocked = _repository.GetBlockedCountries(pageNumber, pageSize);

            if (!string.IsNullOrEmpty(search))
            {
                pagedBlocked = pagedBlocked.Where(c => c.Contains(search, StringComparison.OrdinalIgnoreCase));
            }

            return pagedBlocked;
        }

        public bool IsCountryBlocked(string countryCode)
        {
            if (_temporalBlocks.TryGetValue(countryCode, out var expiry))
            {
                if (expiry > DateTime.UtcNow) return true;
            }

            return _repository.IsCountryBlocked(countryCode);
        }


        public bool AddTemporalBlock(string code, int minutes)
        {
            var expiry = DateTime.UtcNow.AddMinutes(minutes);
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
