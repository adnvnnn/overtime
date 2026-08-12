namespace Overtime.Data.Entities;

public class Address
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public string RecipientName { get; set; } = "";
    public string PhoneNumber { get; set; } = "";
    public string Province { get; set; } = "";
    public string City { get; set; } = "";
    public string District { get; set; } = "";
    public string Address1 { get; set; } = "";
    public string? Address2 { get; set; }
    public string PostalCode { get; set; } = "";
    public bool IsDefault { get; set; }
    public DateTime CreatedAt { get; set; }

    public User? User { get; set; }
}
