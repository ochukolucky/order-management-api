using Microsoft.Extensions.Logging;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;


namespace OrderManagement.Application.Services
{

    public class DiscountService : IDiscountService
    {
        private readonly ILogger<DiscountService> _logger;
        private readonly IOrderRepository _orderRepository;

        public DiscountService(ILogger<DiscountService> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public async Task<decimal> ApplyDiscountAsync(Guid orderId)
        {
            try
            {
                _logger.LogInformation("Fetching order with history for OrderId: {OrderId}", orderId);

                var order = await _orderRepository.GetOrderWithCustomerAndHistoryAsync(orderId);

                if (order == null || order.Customer == null)
                {
                    _logger.LogWarning("Order or customer not found for OrderId: {OrderId}", orderId);
                    throw new ArgumentException("Order or customer not found.");
                }

                var customer = order.Customer;
                decimal discount = 0;

                switch (customer.Segment)
                {
                    case CustomerSegment.VIP:
                        discount = 0.15m;
                        break;
                    case CustomerSegment.NewCustomer:
                        discount = 0.10m;
                        break;
                    case CustomerSegment.Regular:
                        if (customer.Orders.Count > 5)
                            discount = 0.05m;
                        break;
                }

                decimal finalAmount = order.TotalAmount - (order.TotalAmount * discount);

                _logger.LogInformation("Discount applied for OrderId: {OrderId}, Final Amount: {Amount}", order.Id, finalAmount);

                return finalAmount;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error applying discount for OrderId: {OrderId}", orderId);
                throw;
            }
        }
    }
}
