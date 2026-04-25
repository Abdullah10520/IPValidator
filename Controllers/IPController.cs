using IPValidatorAssignment.Models;
using IPValidatorAssignment.Services;
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
        public IPController(IGeolocationService geoService, IBlockedCountryService blockedService)
        {
            _geoService = geoService;
            _blockedService = blockedService;
        }


        [HttpGet("validate")]
        public async Task<IActionResult> ValidateIp([FromQuery] string ip)
        {
            // 1. نجيب كود الدولة من الـ API الخارجي
            var countryCode = await _geoService.GetCountryCodeAsync(ip);

            if (string.IsNullOrEmpty(countryCode))
                return BadRequest("Could not identify country for this IP.");

            // 2. نشيك هل الدولة دي محظورة عندنا في الـ Memory ولا لأ
            bool isBlocked = _blockedService.IsCountryBlocked(countryCode);

            return Ok(new
            {
                Ip = ip,
                Country = countryCode,
                IsBlocked = isBlocked
            });
        }

        [HttpGet("lookup")]
        public IActionResult lookup(string ip)
        {
            var url = $"https://api.ipgeolocation.io/v3/ipgeo?apiKey={"b65c396a9ebf431ebb768087cfe30cfc"}&ip={ip}&fields=location.country_code2,location.country_name,asn";
            var response = new HttpClient().GetAsync(url).Result;

            return Ok(response.Content.ReadAsStringAsync().Result);

        }

        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            // 1. Fetch IP automatically
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();

            if (ip == "::1" || string.IsNullOrEmpty(ip)) //chexk for localhost or if IP is not available
            {
                ip = "156.202.2.2";
            }


            // 2. Fetch UserAgent
            var userAgent = Request.Headers["User-Agent"].ToString();

            // 3. Get CountryCode from Third-party API
            var countryCode = await _geoService.GetCountryCodeAsync(ip);
            if (countryCode == null)
            {
                return BadRequest("Unable to identify IP location.");
            }

            // 4. Check if blocked
            bool isBlocked = _blockedService.IsCountryBlocked(countryCode);

            // 5. Log the attempt 
            if (isBlocked)
            {
                _log.LogAttempt(new BlockedAttemptLog
                {
                    IpAddress = ip,
                    CountryCode = countryCode,
                    Timestamp = DateTime.UtcNow,
                    UserAgent = userAgent,
                    IsBlocked = true
                });
                return Ok(new { Blocked = true, Message = "Your country is blocked." });
            }

            return Ok(new { Blocked = false, Message = "Access granted." });
        }

    }
}
