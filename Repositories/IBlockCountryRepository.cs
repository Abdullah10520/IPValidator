namespace IPValidatorAssignment.Repositories
{
    public interface IBlockCountryRepository
    {
        bool AddCountry(string countryCode);
        bool RemoveCountry(string countryCode);
        IEnumerable<string> GetBlockedCountries(int pageNumber, int pageSize);
        bool IsCountryBlocked(string countryCode);
    }
}
