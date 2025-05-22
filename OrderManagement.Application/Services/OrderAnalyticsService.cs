using Microsoft.Extensions.Logging;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;

namespace OrderManagement.Application.Services
{
    public class OrderAnalyticsService : IOrderAnalyticsService
    {
        private readonly ILogger<OrderAnalyticsService> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderAnalyticsService(
            ILogger<OrderAnalyticsService> logger,
            IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<OrderAnalyticsDto> GetOrderAnalyticsAsync(DateTime? startDate, DateTime? endDate)
        {
            try
            {
                _logger.LogInformation("Computing order analytics for range: {Start} to {End}", startDate, endDate);

                var deliveredOrders = (await _orderRepository.GetDeliveredOrdersAsync())
                    .Where(o =>
                        (!startDate.HasValue || o.CreatedAt >= startDate.Value) &&
                        (!endDate.HasValue || o.CreatedAt <= endDate.Value))
                    .ToList();

                decimal avgValue = deliveredOrders.Any()
                    ? deliveredOrders.Average(o => o.TotalAmount)
                    : 0;

                    double avgHours = deliveredOrders.Any()
                    ? deliveredOrders.Average(o => (o.DeliveredAt.Value - o.CreatedAt).TotalHours)
                    : 0;

                var result = new OrderAnalyticsDto
                {
                    AverageOrderValue = avgValue,
                    AverageFulfillmentTimeInHours = avgHours
                };

                _logger.LogInformation("Analytics computed: AvgValue={AvgValue}, AvgTime={AvgTime}", avgValue, avgHours);
                return result;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to compute order analytics");
                throw;
            }
        }

    }
}