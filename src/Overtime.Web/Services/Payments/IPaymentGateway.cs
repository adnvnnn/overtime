using Overtime.Data.Entities;

namespace Overtime.Web.Services.Payments;

// Result of starting a payment for a freshly-created order. For a real IPG (e.g. Zarinpal),
// Success + RedirectUrl means the caller should send the customer to the gateway; Authority is
// the gateway's token for that attempt, later passed back into VerifyAsync from the callback.
public record PaymentInitiationResult(bool Success, PaymentStatus Status, string? RedirectUrl, string? Authority, string? Error);

// Result of verifying a gateway callback (or, for manual/COD, of the immediate no-op "verification").
public record PaymentVerificationResult(bool Success, string? RefId, string? Error);

// Implemented once per payment provider. Today only ManualPaymentGateway (cash on delivery /
// phone confirmation) is registered. To add a real gateway later: implement this interface
// (e.g. ZarinpalPaymentGateway), add its config/credentials, and swap the DI registration in
// Program.cs — OrderService and the checkout UI do not need to change.
public interface IPaymentGateway
{
    string Name { get; }

    Task<PaymentInitiationResult> InitiateAsync(Order order, string callbackUrl, CancellationToken ct = default);

    Task<PaymentVerificationResult> VerifyAsync(string authority, long amount, CancellationToken ct = default);
}
