using System.Text.Json.Serialization;

namespace IPValidatorAssignment.Models
{
    public class IpGeolocationResponse
    {
        [JsonPropertyName("country_code2")]
        public string CountryCode { get; set; }

        [JsonPropertyName("ip")]
        public string Ip { get; set; }

        // ممكن تضيف دول لو حابب تعرض بيانات أكتر
        [JsonPropertyName("country_name")]
        public string CountryName { get; set; }
    }
}
