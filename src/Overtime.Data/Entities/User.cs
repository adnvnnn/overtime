using System.ComponentModel.DataAnnotations;

namespace Overtime.Data.Entities;

public class User
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = "";
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; }

    public List<Address> Addresses { get; set; } = [];
}
