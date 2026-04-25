using IPValidatorAssignment.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IPValidatorAssignment.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LogsController : ControllerBase
    {
        private readonly ILogService _logService;
        public LogsController(ILogService logService)
        {
            _logService = logService;
        }
        [HttpGet("blocked-attempts")]
        public IActionResult GetBlockedLogs([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
        {
            var allLogs = _logService.GetAllLogs(); // دي بترجع الـ ConcurrentBag كـ List

            var paginatedLogs = allLogs
                .OrderByDescending(l => l.Timestamp) // الأحدث أولاً
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToList();

            return Ok(new
            {
                TotalLogs = allLogs.Count(),
                CurrentPage = page,
                PageSize = pageSize,
                Data = paginatedLogs
            });
        }
    }
}
