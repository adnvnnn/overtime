namespace Overtime.Web.Services;

// Resolves the anonymous cart id (set by the cookie middleware in Program.cs)
// once per circuit, while HttpContext is still available during prerendering.
public class CartContext
{
    public Guid CartId { get; }

    public CartContext(IHttpContextAccessor httpContextAccessor)
    {
        var items = httpContextAccessor.HttpContext?.Items;
        CartId = items is not null && items.TryGetValue(CartCookie.Name, out var value) && value is Guid guid
            ? guid
            : Guid.NewGuid();
    }
}
