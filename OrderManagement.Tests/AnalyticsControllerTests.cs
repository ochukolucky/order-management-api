using Xunit;
using Moq;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using OrderManagement.API.Controllers;
using OrderManagement.Application.DTOs;
using OrderManagement.Application.Interfaces;


namespace OrderManagement.Tests
{
    public class AnalyticsControllerTests
    {
        private readonly Mock<IOrderAnalyticsService> _analyticsServiceMock;
        private readonly AnalyticsController _controller;

        public AnalyticsControllerTests()
        {
            _analyticsServiceMock = new Mock<IOrderAnalyticsService>();
            _controller = new AnalyticsController(_analyticsServiceMock.Object);
        }

        [Fact]
        public async Task GetOrderMetrics_ShouldReturnAnalyticsDto()
        {
            // Arrange
            var startDate = new DateTime(2025, 05, 15);
            var endDate = new DateTime(2025, 05, 22);

            var expectedAnalytics = new OrderAnalyticsDto
            {
                AverageOrderValue = 337.5m,
                AverageFulfillmentTimeInHours = 84
            };

            _analyticsServiceMock
                .Setup(s => s.GetOrderAnalyticsAsync(startDate, endDate))
                .ReturnsAsync(expectedAnalytics);

            // Act
            var result = await _controller.GetOrderMetrics(startDate, endDate);

            // Assert
            var okResult = result.Result as OkObjectResult;
            okResult.Should().NotBeNull();
            okResult!.StatusCode.Should().Be(200);
            okResult.Value.Should().BeEquivalentTo(expectedAnalytics);
        }
    }
}
