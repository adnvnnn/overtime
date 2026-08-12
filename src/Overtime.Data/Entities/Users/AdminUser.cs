using System.ComponentModel.DataAnnotations;

namespace Overtime.Data.Entities;

public class AdminUser
{
    public int Id { get; set; }
    [MaxLength(100)]
    public string Username { get; set; } = "";
    public string PasswordHash { get; set; } = "";
    public string? FullName { get; set; }
    public DateTime CreatedAt { get; set; }
}
