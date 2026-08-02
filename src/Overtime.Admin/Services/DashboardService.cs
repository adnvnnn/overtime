using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Admin.Services;

public record DashboardSummary(long TotalRevenue, int OrderCount, int ProductCount, int CustomerCount);

public record RevenuePoint(DateOnly Date, long Total);

public record TopProduct(string ProductId, string ProductName, int QtySold, long Revenue);

public class DashboardService(IDbContextFactory<AppDbContext> dbFactory)
{
    public async Task<DashboardSummary> GetSummaryAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var totalRevenue = await db.Orders.SumAsync(o => (long?)o.Total) ?? 0;
        var orderCount = await db.Orders.CountAsync();
        var productCount = await db.Products.CountAsync();
        var customerCount = await db.Users.CountAsync();

        return new DashboardSummary(totalRevenue, orderCount, productCount, customerCount);
    }

    public async Task<List<RevenuePoint>> GetRevenueTrendAsync(int days)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var since = DateTime.UtcNow.Date.AddDays(-(days - 1));
        var rows = await db.Orders
            .Where(o => o.CreatedAt >= since)
            .GroupBy(o => o.CreatedAt.Date)
            .Select(g => new { Date = g.Key, Total = g.Sum(o => o.Total) })
            .ToListAsync();

        var byDate = rows.ToDictionary(r => DateOnly.FromDateTime(r.Date), r => r.Total);

        var points = new List<RevenuePoint>();
        for (var i = 0; i < days; i++)
        {
            var date = DateOnly.FromDateTime(since.AddDays(i));
            points.Add(new RevenuePoint(date, byDate.GetValueOrDefault(date)));
        }
        return points;
    }

    public async Task<List<TopProduct>> GetTopProductsAsync(int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();

        var rows = await db.OrderItems
            .GroupBy(i => new { i.ProductId, i.ProductName })
            .Select(g => new
            {
                g.Key.ProductId,
                g.Key.ProductName,
                QtySold = g.Sum(i => i.Qty),
                Revenue = g.Sum(i => i.Qty * i.UnitPrice),
            })
            .OrderByDescending(p => p.QtySold)
            .Take(count)
            .ToListAsync();

        return rows.Select(r => new TopProduct(r.ProductId, r.ProductName, r.QtySold, r.Revenue)).ToList();
    }

    public async Task<List<Order>> GetRecentOrdersAsync(int count)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.Orders
            .OrderByDescending(o => o.CreatedAt)
            .Take(count)
            .ToListAsync();
    }
}
