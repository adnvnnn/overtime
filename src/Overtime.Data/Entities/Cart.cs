namespace Overtime.Data.Entities;

public class Cart
{
    public Guid Id { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<CartItem> Items { get; set; } = [];
}
