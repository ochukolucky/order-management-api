using OrderManagement.Domain.Entities;

namespace OrderManagement.Application.Interfaces
{
    public interface IDiscountService
    {
        Task<decimal> ApplyDiscountAsync(Guid orderId);
    }
}
