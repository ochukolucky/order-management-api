using Microsoft.Extensions.Logging;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Services
{
    public class OrderStatusService : IOrderStatusService
    {
        private readonly ILogger<OrderStatusService> _logger;
        private readonly IOrderRepository _orderRepository;

        public OrderStatusService(ILogger<OrderStatusService> logger, IOrderRepository orderRepository)
        {
            _logger = logger;
            _orderRepository = orderRepository;
        }

        public Task<bool> CanTransitionAsync(OrderStatus currentStatus, OrderStatus newStatus)
        {
            bool isValid = (currentStatus == OrderStatus.Placed && newStatus == OrderStatus.Processing)//Placed → Processing
                || (currentStatus == OrderStatus.Processing && newStatus == OrderStatus.Shipped)//Processing → Shipped
                || (currentStatus == OrderStatus.Shipped && newStatus == OrderStatus.Delivered); //Finally Shipped → Delivered

            //Note : Any other transition (like Delivered → Placed or Processing → Delivered) etc  is considered invalid Transition order.

            return Task.FromResult(isValid);
        }

        public async Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus)
        {
            try
            {
                var order = await _orderRepository.GetOrderByIdAsync(orderId);

                if (order == null)
                    throw new ArgumentException("Order not found");

                _logger.LogInformation("Transitioning OrderId: {OrderId} from {CurrentStatus} to {NewStatus}", order.Id, order.Status, newStatus);

                if (!await CanTransitionAsync(order.Status, newStatus))
                    throw new InvalidOperationException($"Invalid transition from {order.Status} to {newStatus}");

                order.Status = newStatus;

                if (newStatus == OrderStatus.Delivered)
                    order.DeliveredAt = DateTime.UtcNow;

                await _orderRepository.UpdateOrderAsync(order);

                _logger.LogInformation("Order updated in DB: OrderId={OrderId}, Status={NewStatus}", order.Id, newStatus);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating status for OrderId: {OrderId}", orderId);
                throw;
            }
        }
    }
    
}
