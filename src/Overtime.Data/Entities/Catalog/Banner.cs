namespace Overtime.Data.Entities;

public class Banner
{
    public int Id { get; set; }
    public BannerKind Kind { get; set; }
    public string ImageUrl { get; set; } = "";
    public string? Badge { get; set; }
    public string? Title { get; set; }
    public long? Price { get; set; }
    public string? LinkUrl { get; set; }
    public int DisplayOrder { get; set; }
    public bool IsActive { get; set; } = true;
}
