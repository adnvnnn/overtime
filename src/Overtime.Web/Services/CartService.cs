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
            .Include(i => i.Product)
            .ThenInclude(p => p!.Categories)
            .ToListAsync();

        return items
            .Where(i => i.Product is not null)
            .Select(i => new CartLine(
                i.Id, i.ProductId, i.Product!.Name, string.Join("، ", i.Product.Categories.Select(c => c.Name)), i.Product.Price,
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

        var added = await AddItemAsync(db, productId, size.Size, 1, size.StockQty);
        Changed?.Invoke();
        return added > 0;
    }

    // Returns how many units were actually added — may be less than requested (or zero)
    // when the cart already holds all the available stock for that size.
    public async Task<int> AddItemAsync(string productId, int size, int qty)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var stock = await db.ProductSizes
            .Where(s => s.ProductId == productId && s.Size == size)
            .Select(s => (int?)s.StockQty)
            .FirstOrDefaultAsync() ?? 0;

        var added = await AddItemAsync(db, productId, size, qty, stock);
        Changed?.Invoke();
        return added;
    }

    private async Task<int> AddItemAsync(AppDbContext db, string productId, int size, int qty, int stock)
    {
        await EnsureCartAsync(db);
        var existing = await db.CartItems.FirstOrDefaultAsync(i =>
            i.CartId == cartContext.CartId && i.ProductId == productId && i.Size == size);

        var currentQty = existing?.Qty ?? 0;
        var targetQty = Math.Min(stock, currentQty + qty);
        var added = targetQty - currentQty;
        if (added <= 0)
            return 0;

        if (existing is not null)
            existing.Qty = targetQty;
        else
            db.CartItems.Add(new CartItem { CartId = cartContext.CartId, ProductId = productId, Size = size, Qty = targetQty });

        await db.SaveChangesAsync();
        return added;
    }

    // Returns false (without applying the change) when incrementing would exceed available stock.
    public async Task<bool> ChangeQtyAsync(int cartItemId, int delta)
    {
        await using var db = await dbFactory.CreateDbContextAsync();
        var item = await db.CartItems.FirstOrDefaultAsync(i => i.Id == cartItemId && i.CartId == cartContext.CartId);
        if (item is null) return false;

        if (delta > 0)
        {
            var stock = await db.ProductSizes
                .Where(s => s.ProductId == item.ProductId && s.Size == item.Size)
                .Select(s => (int?)s.StockQty)
                .FirstOrDefaultAsync() ?? 0;
            if (item.Qty >= stock)
                return false;
        }

        item.Qty += delta;
        if (item.Qty <= 0)
            db.CartItems.Remove(item);

        await db.SaveChangesAsync();
        Changed?.Invoke();
        return true;
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
