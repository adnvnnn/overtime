using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class OrderService(IDbContextFactory<AppDbContext> dbFactory, CartContext cartContext, CartService cartService, IHttpContextAccessor httpContext)
{
    public async Task<string?> CheckoutAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var items = await db.CartItems
            .Where(i => i.CartId == cartContext.CartId)
            .Include(i => i.Product)
            .ToListAsync();

        if (items.Count == 0)
            return null;

        var userId = GetCurrentUserId();
        var orderNumber = $"OT-{Random.Shared.Next(10000, 99999)}";
        var order = new Order
        {
            OrderNumber = orderNumber,
            CartId = cartContext.CartId,
            UserId = userId,
            CreatedAt = DateTime.UtcNow,
            Total = items.Sum(i => i.Qty * (i.Product?.Price ?? 0)),
        };

        foreach (var item in items)
        {
            order.Items.Add(new OrderItem
            {
                ProductId = item.ProductId,
                ProductName = item.Product?.Name ?? "",
                Size = item.Size,
                Qty = item.Qty,
                UnitPrice = item.Product?.Price ?? 0,
            });

            var size = await db.ProductSizes.FirstOrDefaultAsync(s => s.ProductId == item.ProductId && s.Size == item.Size);
            if (size is not null)
                size.StockQty = Math.Max(0, size.StockQty - item.Qty);
        }

        db.Orders.Add(order);
        db.CartItems.RemoveRange(items);
        await db.SaveChangesAsync();

        cartService.NotifyChanged();
        return orderNumber;
    }

    public async Task<List<Order>> GetUserOrdersAsync(int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    private int? GetCurrentUserId()
    {
        var user = httpContext?.HttpContext?.User;
        if (user?.Identity?.IsAuthenticated != true)
            return null;

        var userIdClaim = user.FindFirst(ClaimTypes.NameIdentifier);
        return userIdClaim != null && int.TryParse(userIdClaim.Value, out var userId) ? userId : null;
    }
}
