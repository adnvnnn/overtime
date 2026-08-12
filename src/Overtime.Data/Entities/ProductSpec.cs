namespace Overtime.Data.Entities;

public class ProductSpec
{
    public int Id { get; set; }
    public int ProductId { get; set; }
    public string Key { get; set; } = "";
    public string Value { get; set; } = "";
    public int SortOrder { get; set; }
}
