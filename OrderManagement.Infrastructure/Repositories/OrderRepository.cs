using Dapper;
using OrderManagement.Application.Interfaces;
using OrderManagement.Domain.Entities;
using OrderManagement.Infrastructure.Data;
using System.Data;

namespace OrderManagement.Infrastructure.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly DapperDbConnectionFactory _connectionFactory;

        public OrderRepository(DapperDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Order>> GetDeliveredOrdersAsync()
        {
            using IDbConnection connection = _connectionFactory.CreateConnection();

            const string sql = @"
            SELECT 
                o.Id, o.CustomerId, o.TotalAmount, o.Status, o.CreatedAt, o.DeliveredAt,
                c.Id, c.Name, c.Segment
            FROM Orders o
            INNER JOIN Customers c ON o.CustomerId = c.Id
            WHERE o.DeliveredAt IS NOT NULL";

            var orders = await connection.QueryAsync<Order, Customer, Order>(
                sql,
                (order, customer) =>
                {
                    order.Customer = customer;
                    return order;
                },
                splitOn: "Id"
            );

            return orders;
        }


        public async Task<Order?> GetOrderWithCustomerAndHistoryAsync(Guid orderId)
        {
            var sql = @"
                SELECT * FROM Orders WHERE Id = @OrderId;
                SELECT * FROM Customers WHERE Id = (SELECT CustomerId FROM Orders WHERE Id = @OrderId);
                SELECT * FROM Orders WHERE CustomerId = (SELECT CustomerId FROM Orders WHERE Id = @OrderId);
            ";

            using var connection = _connectionFactory.CreateConnection();
            using var multi = await connection.QueryMultipleAsync(sql, new { OrderId = orderId });

            var order = await multi.ReadSingleAsync<Order>();
            var customer = await multi.ReadSingleAsync<Customer>();
            var customerOrders = (await multi.ReadAsync<Order>()).ToList();

            customer.Orders = customerOrders;
            order.Customer = customer;

            return order;
        }


        public async Task<Order?> GetOrderByIdAsync(Guid orderId)
        {
            const string sql = @"SELECT * FROM Orders WHERE Id = @Id";

            using var conn = _connectionFactory.CreateConnection();
            return await conn.QueryFirstOrDefaultAsync<Order>(sql, new { Id = orderId });
        }

        public async Task UpdateOrderAsync(Order order)
        {
            const string sql = @"
                UPDATE Orders
                SET Status = @Status, DeliveredAt = @DeliveredAt
                WHERE Id = @Id";

            using var conn = _connectionFactory.CreateConnection();
            await conn.ExecuteAsync(sql, new
            {
                Id = order.Id,
                Status = order.Status,
                DeliveredAt = order.DeliveredAt
            });
        }



    }
}
