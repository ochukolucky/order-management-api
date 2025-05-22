using OrderManagement.Domain.Enums;

namespace OrderManagement.Domain.Entities
{
    public class Customer
    {
        public Guid Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public CustomerSegment Segment { get; set; }

        public ICollection<Order> Orders { get; set; } = new List<Order>();
    }
}
