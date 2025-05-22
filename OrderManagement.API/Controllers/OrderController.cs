using Microsoft.AspNetCore.Mvc;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Enums;

namespace OrderManagement.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class OrderController : ControllerBase
    {
        private readonly IDiscountService _discountService;
        private readonly IOrderStatusService _orderStatusService;

        public OrderController(IDiscountService discountService, IOrderStatusService orderStatusService)
        {
            _discountService = discountService;
            _orderStatusService = orderStatusService;
        }

        [HttpPost("apply-discount")]
        public async Task<ActionResult<decimal>> ApplyDiscount([FromBody] ApplyDiscountDto applyDiscountDto)
        {
            var discountedAmount = await _discountService.ApplyDiscountAsync(applyDiscountDto.orderId);
            return Ok(discountedAmount);
        }

        [HttpPost("update-status")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, OrderStatus newStatus)
        {
            await _orderStatusService.UpdateStatusAsync(orderId, newStatus);
            return Ok("Order status updated successfully.");
        }

    }
}
