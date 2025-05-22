using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AnalyticsController : ControllerBase
    {
        private readonly IOrderAnalyticsService _analyticsService;

        public AnalyticsController(IOrderAnalyticsService analyticsService)
        {
            _analyticsService = analyticsService;
        }
        [HttpGet("order-metrics")]
        public async Task<ActionResult<OrderAnalyticsDto>> GetOrderMetrics(
            [FromQuery] DateTime? startDate,
            [FromQuery] DateTime? endDate)
        {
            var result = await _analyticsService.GetOrderAnalyticsAsync(startDate, endDate);
            return Ok(result);
        }

    }
}
