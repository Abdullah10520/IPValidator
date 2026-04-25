using System.Collections.Concurrent;
using static IPValidatorAssignment.Services.BlockedCountryService;

namespace IPValidatorAssignment.Services
{
    
    public class BlockedCountryService : IBlockedCountryService
    {
    private ConcurrentDictionary<string, byte> _blockedCountries = new();

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
        var pagedBlocked = _blockedCountries.Keys.ToList().Skip((pageNumber - 1) * pageSize).Take(pageSize);
        

        return pagedBlocked;
    }

    public bool IsCountryBlocked(string countryCode)
    {
        return _blockedCountries.ContainsKey(countryCode.ToUpper());
    }
    }
    
}
