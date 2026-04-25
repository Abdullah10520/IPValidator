using IPValidatorAssignment.Models;
using IPValidatorAssignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPValidatorAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly IBlockedCountryService _blockedService;

        public CountriesController(IBlockedCountryService blockedService)
        {
            _blockedService = blockedService;
        }

        [HttpPost("block")]
        public IActionResult AddblockedCountry([FromBody] string countryCode)
        {
            var result = _blockedService.AddCountry(countryCode);
            if (result)
            { 
                return Ok("Country blocked.");
            }
            else
            {
                return BadRequest("Country already blocked.");
            }
        }

        [HttpDelete("block/{countryCode}")]
        public IActionResult DeleteBlockedCountry(string countryCode)
        {
            var result = _blockedService.RemoveCountry(countryCode);
            return result ? Ok("Country removed.") : NotFound("Country not exist as blocked.");
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries(string? search, int pageNumber, int pageSize)
        {
            var pageBlockedCountries = _blockedService.GetBlockedCountries(search, pageNumber, pageSize);
            
            return Ok(pageBlockedCountries);
        }
        [HttpPost("temporal-block")]
        public IActionResult TemporalBlock([FromBody] TemporalBlockRequest request)
        {
            if (request.DurationMinutes < 1 || request.DurationMinutes > 1440)
                return BadRequest("Duration must be between 1 and 1440 minutes.");

            if (string.IsNullOrEmpty(request.CountryCode) || request.CountryCode.Length != 2)
                return BadRequest("Invalid country code format.");

            var added = _blockedService.AddTemporalBlock(request.CountryCode, request.DurationMinutes);

            if (!added)
                return Conflict("This country is already temporarily blocked.");

            return Ok($"Country {request.CountryCode} blocked for {request.DurationMinutes} minutes.");
        }
    }
}
