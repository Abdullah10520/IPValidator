using IPValidatorAssignment.Models;

namespace IPValidatorAssignment.Services
{
    public interface IBlockedCountryService
    {
        bool AddCountry(string countryCode);
        bool RemoveCountry(string countryCode);
        IEnumerable<string> GetBlockedCountries(string? search, int pageNumber, int pageSize);
        bool IsCountryBlocked(string countryCode);


        bool AddTemporalBlock(string code, int minutes);
        void RemoveExpiredBlocks();
    }
}
