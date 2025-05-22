using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace OrderManagement.Application.Interfaces
{
    public interface IOrderStatusService
    {
        Task<bool> CanTransitionAsync(OrderStatus currentStatus, OrderStatus newStatus);
        Task UpdateStatusAsync(Guid orderId, OrderStatus newStatus);
    }
}
