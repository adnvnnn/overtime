using System.ComponentModel.DataAnnotations;

namespace Overtime.Data.Entities;

public class OtpCode
{
    public int Id { get; set; }
    [MaxLength(20)]
    public string PhoneNumber { get; set; } = "";
    public string Code { get; set; } = "";
    public DateTime CreatedAt { get; set; }
    public DateTime ExpiresAt { get; set; }
    public DateTime? ConsumedAt { get; set; }
    public int Attempts { get; set; }
}
