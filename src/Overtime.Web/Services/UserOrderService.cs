using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public class UserOrderService(IDbContextFactory<AppDbContext> dbFactory, IHttpContextAccessor httpContext)
{
    public async Task<List<Order>> GetMyOrdersAsync()
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return [];

        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Orders
            .Where(o => o.UserId == userId)
            .Include(o => o.Items)
            .OrderByDescending(o => o.CreatedAt)
            .ToListAsync();
    }

    public async Task<Order?> GetOrderByIdAsync(int orderId)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return null;

        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Orders
            .Where(o => o.Id == orderId && o.UserId == userId)
            .Include(o => o.Items)
            .FirstOrDefaultAsync();
    }

    public async Task<bool> RequestCancellationAsync(int orderId, string reason)
    {
        var userId = GetCurrentUserId();
        if (userId is null)
            return false;

        await using var db = await dbFactory.CreateDbContextAsync();
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

        if (order is null)
            return false;

        if (order.Status == OrderStatus.Delivered || order.Status == OrderStatus.Cancelled)
            return false;

        order.Status = OrderStatus.Cancelled;
        order.CancellationReason = reason;
        order.CancelledAt = DateTime.UtcNow;
        await db.SaveChangesAsync();

        return true;
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
