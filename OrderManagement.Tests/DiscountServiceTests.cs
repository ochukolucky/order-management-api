using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using OrderManagement.Application.Services;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Domain.Enums;

namespace OrderManagement.Tests
{
    public class DiscountServiceTests
    {
        private readonly Mock<IOrderRepository> _mockRepo;
        private readonly DiscountService _service;

        public DiscountServiceTests()
        {
            var logger = new Mock<ILogger<DiscountService>>();
            _mockRepo = new Mock<IOrderRepository>();
            _service = new DiscountService(logger.Object, _mockRepo.Object);
        }

        [Fact]
        public async Task ApplyDiscountAsync_ShouldApply15Percent_ForVIP()
        {
            // Arrange
            var orderId = Guid.Parse("621E0FFC-81F3-458D-8C0F-0AAAAA845823");

            var order = new Order
            {
                Id = orderId,
                TotalAmount = 1000,
                Customer = new Customer
                {
                    Id = Guid.NewGuid(),
                    Segment = CustomerSegment.VIP,
                    Orders = new List<Order>() // Order history not needed for VIP
                }
            };

            _mockRepo.Setup(repo => repo.GetOrderWithCustomerAndHistoryAsync(orderId))
                     .ReturnsAsync(order);

            // Act
            var result = await _service.ApplyDiscountAsync(orderId);

            // Assert
            result.Should().Be(850); // 15% of 1000 = 150 discount
        }

        [Fact]
        public async Task ApplyDiscountAsync_ShouldApply5Percent_ForRegularWithMoreThan5Orders()
        {
            // Arrange
            var orderId = Guid.NewGuid();

            var customer = new Customer
            {
                Id = Guid.NewGuid(),
                Segment = CustomerSegment.Regular,
                Orders = Enumerable.Range(1, 6).Select(_ => new Order()).ToList()
            };

            var order = new Order
            {
                Id = orderId,
                TotalAmount = 200,
                Customer = customer
            };

            _mockRepo.Setup(repo => repo.GetOrderWithCustomerAndHistoryAsync(orderId))
                     .ReturnsAsync(order);

            // Act
            var result = await _service.ApplyDiscountAsync(orderId);

            // Assert
            result.Should().Be(190); // 5% of 200 = 10 discount
        }


    }
}
