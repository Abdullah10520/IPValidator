using IPValidatorAssignment.Models;

namespace IPValidatorAssignment.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private string _apiKey = Environment.GetEnvironmentVariable("ApiKey");

        public GeolocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCountryCodeAsync(string ip)
        {
            var url = $"ipgeo?apiKey={_apiKey}&ip={ip}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<IpGeolocationResponse>();
                return data?.CountryCode; 
            }

            return null;
        }
    }
}
