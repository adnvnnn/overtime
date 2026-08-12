using Overtime.Data.Entities;

namespace Overtime.Web.Services.Payments;

// Stand-in "gateway" used until a real online IPG (e.g. Zarinpal) is configured: no money moves
// here, the order is simply marked as awaiting confirmation and fulfilled cash-on-delivery /
// via a follow-up call, same as the shop already does manually today.
public class ManualPaymentGateway : IPaymentGateway
{
    public string Name => "Manual";

    public Task<PaymentInitiationResult> InitiateAsync(Order order, string callbackUrl, CancellationToken ct = default) =>
        Task.FromResult(new PaymentInitiationResult(true, PaymentStatus.AwaitingConfirmation, null, null, null));

    public Task<PaymentVerificationResult> VerifyAsync(string authority, long amount, CancellationToken ct = default) =>
        Task.FromResult(new PaymentVerificationResult(false, null, "پرداخت آنلاین هنوز فعال نشده است."));
}
