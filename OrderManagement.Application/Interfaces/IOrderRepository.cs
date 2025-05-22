using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderRepository
    {
        Task<IEnumerable<Order>> GetDeliveredOrdersAsync();
        Task<Order?> GetOrderWithCustomerAndHistoryAsync(Guid orderId);
        Task<Order?> GetOrderByIdAsync(Guid orderId);
        Task UpdateOrderAsync(Order order);

    }
}
