namespace Overtime.Data.Entities;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = "";
    public Guid CartId { get; set; }
    public DateTime CreatedAt { get; set; }
    public long Total { get; set; }
    public OrderStatus Status { get; set; }
    public int? AddressId { get; set; }
    public int? UserId { get; set; }

    public List<OrderItem> Items { get; set; } = [];
    public Address? Address { get; set; }
    public User? User { get; set; }
}
