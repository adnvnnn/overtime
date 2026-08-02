namespace Overtime.Data.Entities;

public class OrderItem
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string ProductId { get; set; } = "";
    public string ProductName { get; set; } = "";
    public int Size { get; set; }
    public int Qty { get; set; }
    public long UnitPrice { get; set; }
}
