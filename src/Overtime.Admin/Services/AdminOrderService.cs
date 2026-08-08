using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public class AdminOrderService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<List<Order>> GetAllAsync(OrderStatus? status = null, string? query = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        IQueryable<Order> q = db.Orders.Include(o => o.Items);

        if (status is not null)
            q = q.Where(o => o.Status == status);

        if (!string.IsNullOrWhiteSpace(query))
        {
            var pattern = $"%{query.Trim()}%";
            q = q.Where(o => EF.Functions.Like(o.OrderNumber, pattern));
        }

        return await q.OrderByDescending(o => o.CreatedAt).ToListAsync();
    }

    public async Task<Order?> GetByIdAsync(int id)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Orders.Include(o => o.Items).FirstOrDefaultAsync(o => o.Id == id);
    }

    public async Task UpdateStatusAsync(int orderId, OrderStatus status)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is not null)
        {
            order.Status = status;
            if (status == OrderStatus.Cancelled && order.CancelledAt is null)
            {
                order.CancelledAt = DateTime.UtcNow;
            }
            await db.SaveChangesAsync();
        }
    }

    public async Task CancelOrderAsync(int orderId, string? reason = null)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var order = await db.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        if (order is not null)
        {
            order.Status = OrderStatus.Cancelled;
            order.CancellationReason = reason;
            order.CancelledAt = DateTime.UtcNow;
            await db.SaveChangesAsync();
        }
    }
}
