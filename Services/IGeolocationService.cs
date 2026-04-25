namespace IPValidatorAssignment.Services
{
    public interface IGeolocationService
    {
        public Task<string> GetCountryCodeAsync(string ip);
    }
}
