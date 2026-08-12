namespace Overtime.Data.Entities;

// One row per gateway attempt against an Order. Manual (cash-on-delivery style) checkouts
// still get a row here so the order's payment history is uniform once a real gateway is wired in.
public class Payment
{
    public int Id { get; set; }
    public int OrderId { get; set; }
    public string Gateway { get; set; } = "";
    public long Amount { get; set; }
    public PaymentStatus Status { get; set; }
    public string? Authority { get; set; }
    public string? RefId { get; set; }
    public string? ErrorMessage { get; set; }
    public DateTime CreatedAt { get; set; }
    public DateTime? VerifiedAt { get; set; }

    public Order? Order { get; set; }
}
