namespace Overtime.Data.Entities;

public class Brand
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public string Slug { get; set; } = "";
    public string? LogoUrl { get; set; }
    public int DisplayOrder { get; set; }

    public List<Product> Products { get; set; } = [];
}
