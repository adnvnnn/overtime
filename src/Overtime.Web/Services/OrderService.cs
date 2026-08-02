using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class OrderService(IDbContextFactory<AppDbContext> dbFactory, CartContext cartContext, CartService cartService)
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

        var orderNumber = $"OT-{Random.Shared.Next(10000, 99999)}";
        var order = new Order
        {
            OrderNumber = orderNumber,
            CartId = cartContext.CartId,
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
}
