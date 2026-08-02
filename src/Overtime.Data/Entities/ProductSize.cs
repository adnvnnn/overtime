namespace Overtime.Data.Entities;

public class ProductSize
{
    public int Id { get; set; }
    public string ProductId { get; set; } = "";
    public int Size { get; set; }
    public int StockQty { get; set; }
}
