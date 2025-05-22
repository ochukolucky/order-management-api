using OrderManagement.Application.DTOs;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderAnalyticsService
    {
        Task<OrderAnalyticsDto> GetOrderAnalyticsAsync(DateTime? startDate, DateTime? endDate);

    }
}
