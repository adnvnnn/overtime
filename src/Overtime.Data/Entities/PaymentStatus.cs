namespace Overtime.Data.Entities;

public enum PaymentStatus
{
    AwaitingConfirmation,
    AwaitingGateway,
    Paid,
    Failed,
}
