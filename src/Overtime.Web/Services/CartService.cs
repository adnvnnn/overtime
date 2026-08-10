using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Data.Entities;

namespace Overtime.Web.Services;

public record CartLine(int CartItemId, string ProductId, string Name, string Category, long Price, string Swatch, string? Image, int Size, int Qty)
{
    public long LineTotal => Price * Qty;
}

public class CartService(IDbContextFactory<AppDbContext> dbFactory, CartContext cartContext)
{
    public event Action? Changed;

    public bool IsDrawerOpen { get; private set; }

    public void OpenDrawer()
    {
        IsDrawerOpen = true;
        Changed?.Invoke();
    }

    public void CloseDrawer()
    {
        IsDrawerOpen = false;
        Changed?.Invoke();
    }

    public void ToggleDrawer()
    {
        IsDrawerOpen = !IsDrawerOpen;
        Changed?.Invoke();
    }

    public async Task<List<CartLine>> GetLinesAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var items = await db.CartItems
            .Where(i => i.CartId == cartContext.CartId)
            .Include(i => i.Product)
            .ThenInclude(p => p!.Images.OrderBy(img => img.SortOrder))
            .ToListAsync();

        return items
            .Where(i => i.Product is not null)
            .Select(i => new CartLine(
                i.Id, i.ProductId, i.Product!.Name, i.Product.Category, i.Product.Price,
                i.Product.Swatch, i.Product.Images.FirstOrDefault()?.Url, i.Size, i.Qty))
            .ToList();
    }

    public async Task<int> GetCountAsync()
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        return await db.CartItems.Where(i => i.CartId == cartContext.CartId).SumAsync(i => (int?)i.Qty) ?? 0;
    }

    // Used by the one-click "add" button on product cards (no size picker there):
    // picks the smallest still-in-stock size automatically.
    public async Task<bool> QuickAddAsync(string productId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var size = await db.ProductSizes
            .Where(s => s.ProductId == productId && s.StockQty > 0)
            .OrderBy(s => s.Size)
            .FirstOrDefaultAsync();
        if (size is null) return false;

        await AddItemAsync(db, productId, size.Size, 1);
        Changed?.Invoke();
        return true;
    }

    public async Task AddItemAsync(string productId, int size, int qty)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        await AddItemAsync(db, productId, size, qty);
        Changed?.Invoke();
    }

    private async Task AddItemAsync(AppDbContext db, string productId, int size, int qty)
    {
        await EnsureCartAsync(db);
        var existing = await db.CartItems.FirstOrDefaultAsync(i =>
            i.CartId == cartContext.CartId && i.ProductId == productId && i.Size == size);
        if (existing is not null)
            existing.Qty += qty;
        else
            db.CartItems.Add(new CartItem { CartId = cartContext.CartId, ProductId = productId, Size = size, Qty = qty });

        await db.SaveChangesAsync();
    }

    public async Task ChangeQtyAsync(int cartItemId, int delta)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var item = await db.CartItems.FirstOrDefaultAsync(i => i.Id == cartItemId && i.CartId == cartContext.CartId);
        if (item is null) return;

        item.Qty += delta;
        if (item.Qty <= 0)
            db.CartItems.Remove(item);

        await db.SaveChangesAsync();
        Changed?.Invoke();
    }

    public async Task RemoveItemAsync(int cartItemId)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var item = await db.CartItems.FirstOrDefaultAsync(i => i.Id == cartItemId && i.CartId == cartContext.CartId);
        if (item is null) return;

        db.CartItems.Remove(item);
        await db.SaveChangesAsync();
        Changed?.Invoke();
    }

    public void NotifyChanged() => Changed?.Invoke();

    private async Task EnsureCartAsync(AppDbContext db)
    {
        var exists = await db.Carts.AnyAsync(c => c.Id == cartContext.CartId);
        if (!exists)
            db.Carts.Add(new Cart { Id = cartContext.CartId, CreatedAt = DateTime.UtcNow });
    }
}
