using System.ComponentModel.DataAnnotations;

namespace Overtime.Data.Entities;

public class Review
{
    public int Id { get; set; }
    [MaxLength(50)]
    public string ProductId { get; set; } = "";
    public int UserId { get; set; }
    public int Rating { get; set; }
    public string? Comment { get; set; }
    public DateTime CreatedAt { get; set; }

    public Product? Product { get; set; }
    public User? User { get; set; }
}
