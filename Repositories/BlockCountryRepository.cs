using System.Collections.Concurrent;

namespace IPValidatorAssignment.Repositories
{
    public class BlockCountryRepository : IBlockCountryRepository
    {
        private ConcurrentDictionary<string, byte> _blockedCountries = new();
        private ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        public bool AddCountry(string countryCode)
        {
            return _blockedCountries.TryAdd(countryCode.ToUpper(), 0);
        }
        public bool RemoveCountry(string countryCode)
        {
            return _blockedCountries.TryRemove(countryCode.ToUpper(), out _);
        }
        public IEnumerable<string> GetBlockedCountries(int pageNumber, int pageSize)
        {
            return _blockedCountries.Keys.Skip((pageNumber - 1) * pageSize).Take(pageSize);
        }
        public bool IsCountryBlocked(string countryCode)
        {
            return _blockedCountries.ContainsKey(countryCode.ToUpper());
        }
    }
}
