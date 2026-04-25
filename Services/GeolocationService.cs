using IPValidatorAssignment.Models;

namespace IPValidatorAssignment.Services
{
    public class GeolocationService : IGeolocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey = "b65c396a9ebf431ebb768087cfe30cfc"; // حط مفتاحك هنا

        public GeolocationService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<string> GetCountryCodeAsync(string ip)
        {
            // الرابط بيبقى: /ipgeo?apiKey=...&ip=...
            var url = $"ipgeo?apiKey={_apiKey}&ip={ip}";
            //var url = $"ipgeo?ip={ip}";

            var response = await _httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var data = await response.Content.ReadFromJsonAsync<IpGeolocationResponse>();
                return data?.CountryCode; // هيرجع كود الدولة (مثل US)
            }

            return null;
        }
    }
}
