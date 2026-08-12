namespace Overtime.Data.Entities;

// Explicit join entity for the Product<->Usage many-to-many relationship
// (rather than EF's implicit skip-navigation join table) so the FK column
// names are declared, not inferred by convention.
public class ProductUsage
{
    public int ProductId { get; set; }
    public int UsageId { get; set; }
}
