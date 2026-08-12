namespace Overtime.Data.Entities;

public class Usage
{
    public int Id { get; set; }
    public string Name { get; set; } = "";
    public int DisplayOrder { get; set; }

    public List<Product> Products { get; set; } = [];
}
