namespace Overtime.Data.Entities;

public class CartItem
{
    public int Id { get; set; }
    public Guid CartId { get; set; }
    public int ProductId { get; set; }
    public int Size { get; set; }
    public int Qty { get; set; }

    public Product? Product { get; set; }
}
