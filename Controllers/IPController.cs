using IPValidatorAssignment.Models;
using IPValidatorAssignment.Services;
using Microsoft.AspNetCore.DataProtection.KeyManagement;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net;

namespace IPValidatorAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class IPController : ControllerBase
    {

        private readonly IGeolocationService _geoService;
        private readonly IBlockedCountryService _blockedService;
        private readonly ILogService _log;
        public IPController(IGeolocationService geoService, IBlockedCountryService blockedService, ILogService logService)
        {
            _geoService = geoService;
            _blockedService = blockedService;
            _log = logService;
        }        

        [HttpGet("lookup")]
        public IActionResult lookup(string ip)
        {
            var apiKey = Environment.GetEnvironmentVariable("ApiKey");

            if (!System.Net.IPAddress.TryParse(ip, out _))
                return BadRequest("Invalid IP format.");

            var url = $"https://api.ipgeolocation.io/v3/ipgeo?apiKey={apiKey}&ip={ip}&fields=location.country_code2,location.country_name,asn";
            var response = new HttpClient().GetAsync(url).Result;

            return Ok(response.Content.ReadAsStringAsync().Result);

        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ip == "::1" || string.IsNullOrEmpty(ip)) 
            {
                ip = "156.202.2.2";
            }


            var userAgent = Request.Headers["User-Agent"].ToString();

            var countryCode = await _geoService.GetCountryCodeAsync(ip);
            if (countryCode == null)
            {
                return BadRequest("Unable to identify IP location.");
            }

            bool isBlocked = _blockedService.IsCountryBlocked(countryCode);

            _log.LogAttempt(new BlockedAttemptLog
            {
                IpAddress = ip,
                CountryCode = countryCode,
                Timestamp = DateTime.UtcNow,
                UserAgent = userAgent,
                IsBlocked = isBlocked
            });

            if (isBlocked)
            {
                // if we want to log blocked attempts, we can uncomment this section

                //_log.LogAttempt(new BlockedAttemptLog
                //{
                //    IpAddress = ip,
                //    CountryCode = countryCode,
                //    Timestamp = DateTime.UtcNow,
                //    UserAgent = userAgent,
                //    IsBlocked = true
                //});
                return Ok(new { Blocked = true, Message = "Your country is blocked." });
            }

            return Ok(new { Blocked = false, Message = "Access granted." });
        }

    }
}
