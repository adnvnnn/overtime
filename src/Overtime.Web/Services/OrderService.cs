using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Overtime.Data;
using Overtime.Data.Entities;
using Overtime.Web.Resources;

namespace Overtime.Web.Services;

public record OrderLineItem(string ProductName, int Size, int Qty, long UnitPrice);
public record OrderSummary(int Id, string OrderNumber, DateTime CreatedAt, long Total, OrderStatus Status, List<OrderLineItem> Items, string? AddressSummary);

public class OrderService(IDbContextFactory<AppDbContext> dbFactory, CartContext cartContext, CartService cartService, IStringLocalizer<AppStrings> localizer)
{
    public async Task<List<OrderSummary>> GetUserOrdersAsync(int userId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var orders = await db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .Include(o => o.Address)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();

        return orders.Select(o => new OrderSummary(
            o.Id,
            o.OrderNumber,
            o.CreatedAt,
            o.Total,
            o.Status,
            o.Items.Select(i => new OrderLineItem(i.ProductName, i.Size, i.Qty, i.UnitPrice)).ToList(),
            o.Address is null ? null : $"{o.Address.Province}، {o.Address.City}، {o.Address.District}"
        )).ToList();
    }

    public async Task<(string? OrderNumber, string? Error)> CheckoutAsync(int? userId = null, int? addressId = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var items = await db.CartItems
            .Where(i => i.CartId == cartContext.CartId)
            .Include(i => i.Product)
            .ToListAsync();

        if (items.Count == 0)
            return (null, localizer["Order_EmptyCartError"]);

        var sizes = await db.ProductSizes
            .Where(s => items.Select(i => i.ProductId).Contains(s.ProductId))
            .ToListAsync();

        foreach (var item in items)
        {
            var size = sizes.FirstOrDefault(s => s.ProductId == item.ProductId && s.Size == item.Size);
            if (size is null || size.StockQty < item.Qty)
                return (null, localizer["Order_InsufficientStockFormat", item.Product?.Name ?? ""]);
        }

        var orderNumber = $"OT-{Random.Shared.Next(10000, 99999)}";
        var order = new Order
        {
            OrderNumber = orderNumber,
            CartId = cartContext.CartId,
            CreatedAt = DateTime.UtcNow,
            Total = items.Sum(i => i.Qty * (i.Product?.Price ?? 0)),
            UserId = userId,
            AddressId = addressId,
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

            var size = sizes.First(s => s.ProductId == item.ProductId && s.Size == item.Size);
            size.StockQty -= item.Qty;
        }

        db.Orders.Add(order);
        db.CartItems.RemoveRange(items);
        await db.SaveChangesAsync();

        cartService.NotifyChanged();
        return (orderNumber, null);
    }
}
