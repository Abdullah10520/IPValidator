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
            return result ? Ok("Country blocked.") : BadRequest("Country already blocked.");
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
            var pageBlockedCountries = _blockedService.GetBlockedCountries(pageNumber, pageSize);
            if (!string.IsNullOrEmpty(search))
            {
                pageBlockedCountries = pageBlockedCountries.Where(c => c.Contains(search, StringComparison.OrdinalIgnoreCase));
            }
            return Ok(pageBlockedCountries);
        }
    }
}
