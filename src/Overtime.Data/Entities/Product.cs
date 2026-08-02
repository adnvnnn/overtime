namespace Overtime.Data.Entities;

public class Product
{
    public string Id { get; set; } = "";
    public string Name { get; set; } = "";
    public string Category { get; set; } = "";
    public long Price { get; set; }
    public string Swatch { get; set; } = "accent";
    public string? Badge { get; set; }
    public double Rating { get; set; }
    public int ReviewCount { get; set; }
    public string Description { get; set; } = "";
    public int DisplayOrder { get; set; }

    public List<ProductImage> Images { get; set; } = [];
    public List<ProductSpec> Specs { get; set; } = [];
    public List<ProductSize> Sizes { get; set; } = [];
}
