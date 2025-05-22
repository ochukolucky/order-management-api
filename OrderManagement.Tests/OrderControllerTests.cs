using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Controllers;
using OrderManagement.Application.Interfaces;
using OrderManagement.Application.DTOs;
using OrderManagement.Domain.Enums;


namespace OrderManagement.Tests
{
    public class OrderControllerTests
    {
        private readonly Mock<IDiscountService> _discountServiceMock;
        private readonly Mock<IOrderStatusService> _orderStatusServiceMock;
        private readonly OrderController _controller;

        public OrderControllerTests()
        {
            _discountServiceMock = new Mock<IDiscountService>();
            _orderStatusServiceMock = new Mock<IOrderStatusService>();

            _controller = new OrderController(
                _discountServiceMock.Object,
                _orderStatusServiceMock.Object
            );
        }

        [Fact]
        public async Task ApplyDiscount_ShouldReturnDiscountedAmount()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var expectedDiscount = 850m;

            _discountServiceMock
                .Setup(s => s.ApplyDiscountAsync(orderId))
                .ReturnsAsync(expectedDiscount);

            var dto = new ApplyDiscountDto { orderId = orderId };

            // Act
            var result = await _controller.ApplyDiscount(dto);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().Be(expectedDiscount);
        }

        [Fact]
        public async Task UpdateOrderStatus_ShouldReturnSuccessMessage()
        {
            // Arrange
            var orderId = Guid.NewGuid();
            var newStatus = OrderStatus.Processing;

            _orderStatusServiceMock
                .Setup(s => s.UpdateStatusAsync(orderId, newStatus))
                .Returns(Task.CompletedTask);

            // Act
            var result = await _controller.UpdateOrderStatus(orderId, newStatus);

            // Assert
            var okResult = result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().Be("Order status updated successfully.");
        }
    }
}
