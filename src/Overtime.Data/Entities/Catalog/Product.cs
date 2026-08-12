using System.ComponentModel.DataAnnotations;

namespace Overtime.Data.Entities;

public class Product
{
    public int Id { get; set; }
    [MaxLength(80)]
    public string Slug { get; set; } = "";
    public string Name { get; set; } = "";
    public int BrandId { get; set; }
    public Brand? Brand { get; set; }
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
    public List<Category> Categories { get; set; } = [];
    public List<Usage> Usages { get; set; } = [];
}
