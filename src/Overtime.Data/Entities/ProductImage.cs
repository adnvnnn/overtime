namespace Overtime.Data.Entities;

public class ProductImage
{
    public int Id { get; set; }
    public string ProductId { get; set; } = "";
    public string Url { get; set; } = "";
    public int SortOrder { get; set; }
}
