using Microsoft.EntityFrameworkCore;
using Overtime.Data.Entities;

namespace Overtime.Data;

public class AppDbContext(DbContextOptions<AppDbContext> options) : DbContext(options)
{
    public DbSet<Brand> Brands => Set<Brand>();
    public DbSet<Product> Products => Set<Product>();
    public DbSet<ProductImage> ProductImages => Set<ProductImage>();
    public DbSet<ProductSpec> ProductSpecs => Set<ProductSpec>();
    public DbSet<ProductSize> ProductSizes => Set<ProductSize>();
    public DbSet<Cart> Carts => Set<Cart>();
    public DbSet<CartItem> CartItems => Set<CartItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<User> Users => Set<User>();
    public DbSet<Address> Addresses => Set<Address>();
    public DbSet<OtpCode> OtpCodes => Set<OtpCode>();
    public DbSet<AdminUser> AdminUsers => Set<AdminUser>();
    public DbSet<Review> Reviews => Set<Review>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Brand>(e =>
        {
            e.HasIndex(b => b.Slug).IsUnique();
            e.HasData(SeedData.GetBrands());
        });

        modelBuilder.Entity<Product>(e =>
        {
            e.HasOne(p => p.Brand).WithMany(b => b.Products).HasForeignKey(p => p.BrandId).OnDelete(DeleteBehavior.Restrict);
            e.HasMany(p => p.Images).WithOne().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(p => p.Specs).WithOne().HasForeignKey(s => s.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasMany(p => p.Sizes).WithOne().HasForeignKey(s => s.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasData(SeedData.GetProducts());
        });

        modelBuilder.Entity<ProductImage>().HasData(SeedData.GetImages());
        modelBuilder.Entity<ProductSpec>().HasData(SeedData.GetSpecs());
        modelBuilder.Entity<ProductSize>(e =>
        {
            e.HasIndex(s => new { s.ProductId, s.Size }).IsUnique();
            e.HasData(SeedData.GetSizes());
        });

        modelBuilder.Entity<Cart>()
            .HasMany(c => c.Items).WithOne().HasForeignKey(i => i.CartId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<CartItem>(e =>
        {
            e.HasIndex(i => new { i.CartId, i.ProductId, i.Size }).IsUnique();
            e.HasOne(i => i.Product).WithMany().HasForeignKey(i => i.ProductId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Order>()
            .HasMany(o => o.Items).WithOne().HasForeignKey(i => i.OrderId).OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>(e =>
        {
            e.HasIndex(u => u.PhoneNumber).IsUnique();
            e.HasMany(u => u.Addresses).WithOne(a => a.User).HasForeignKey(a => a.UserId).OnDelete(DeleteBehavior.Cascade);
        });

        modelBuilder.Entity<Address>().HasIndex(a => new { a.UserId, a.IsDefault });

        modelBuilder.Entity<Order>(e =>
        {
            e.HasOne(o => o.Address).WithMany().HasForeignKey(o => o.AddressId).OnDelete(DeleteBehavior.SetNull);
            e.HasOne(o => o.User).WithMany().HasForeignKey(o => o.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<OtpCode>()
            .HasIndex(o => new { o.PhoneNumber, o.CreatedAt });

        modelBuilder.Entity<Review>(e =>
        {
            e.HasIndex(r => new { r.ProductId, r.UserId }).IsUnique();
            e.HasOne(r => r.Product).WithMany().HasForeignKey(r => r.ProductId).OnDelete(DeleteBehavior.Cascade);
            e.HasOne(r => r.User).WithMany().HasForeignKey(r => r.UserId).OnDelete(DeleteBehavior.NoAction);
        });

        modelBuilder.Entity<AdminUser>(e =>
        {
            e.Property(a => a.Username).UseCollation("SQL_Latin1_General_CP1_CI_AS");
            e.HasIndex(a => a.Username).IsUnique();
        });
    }
}
