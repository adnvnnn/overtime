using Overtime.Data.Entities;

namespace Overtime.Data;

public static class SeedData
{
    private sealed record ProductSeed(
        string Id, string Name, string Category, string Usage, long Price, string Swatch, string? Badge,
        double Rating, int ReviewCount, string Description, string Image,
        (string Key, string Value)[] Specs, int[] Sizes);

    private static readonly ProductSeed[] Products =
    [
        new("air-pulse", "Air Pulse", "مردانه", "رانینگ", 2950000, "accent", "پرفروش", 4.8, 124,
            "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.",
            "assets/images/products/product-1.jpg",
            [("جنس رویه", "مش تنفس‌پذیر"), ("نوع بند", "بندی معمولی"), ("کاربرد", "دویدن و پیاده‌روی"), ("رنگ اصلی", "بنفش")],
            [40, 41, 42, 43, 44, 45]),

        new("highjump-x", "Highjump X", "مردانه", "بسکتبال", 3800000, "accent-2", "جدید", 4.7, 98,
            "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.",
            "assets/images/products/product-2.jpg",
            [("جنس رویه", "سینتتیک و مش"), ("نوع بند", "بندی بلند"), ("کاربرد", "بسکتبال"), ("رنگ اصلی", "بنفش تیره")],
            [41, 42, 43, 44, 45, 46]),

        new("street-low", "Street Low", "یونیسکس", "روزمره", 2200000, "ink", null, 4.6, 156,
            "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.",
            "assets/images/products/product-3.jpg",
            [("جنس رویه", "چرم مصنوعی"), ("نوع بند", "بندی معمولی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "مشکی")],
            [38, 39, 40, 41, 42, 43, 44]),

        new("aria-knit", "Aria Knit", "زنانه", "نیت", 2650000, "accent-2", "تخفیف", 4.8, 142,
            "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.",
            "assets/images/products/product-4.jpg",
            [("جنس رویه", "نیت کشباف"), ("نوع بند", "بندی کشی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "بنفش تیره")],
            [36, 37, 38, 39, 40]),

        new("mini-dash", "Mini Dash", "بچگانه", "روزمره", 1850000, "accent", null, 4.9, 86,
            "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.",
            "assets/images/products/product-5.jpg",
            [("جنس رویه", "مش نرم"), ("نوع بند", "بست چسبی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "بنفش")],
            [28, 29, 30, 31, 32, 33, 34]),

        new("overtime-06", "Overtime №06", "لیمیتد ادیشن", "کالکشنی", 4500000, "ink", "لیمیتد", 5.0, 41,
            "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.",
            "assets/images/products/product-6.jpg",
            [("جنس رویه", "چرم و سوئد"), ("نوع بند", "بندی معمولی"), ("کاربرد", "کالکشنی و روزمره"), ("رنگ اصلی", "مشکی و بنفش")],
            [40, 41, 42, 43, 44]),
    ];

    public static Brand[] GetBrands() =>
    [
        new() { Id = 1, Name = "اورتایم", Slug = "overtime", LogoUrl = null, DisplayOrder = 0 },
        new() { Id = 2, Name = "Nike", Slug = "nike", LogoUrl = "assets/images/brands/nike-logo.svg", DisplayOrder = 1 },
        new() { Id = 3, Name = "Adidas", Slug = "adidas", LogoUrl = "assets/images/brands/adidas-logo.svg", DisplayOrder = 2 },
        new() { Id = 4, Name = "Air Jordan", Slug = "airjordan", LogoUrl = "assets/images/brands/airjordan-logo.svg", DisplayOrder = 3 },
        new() { Id = 5, Name = "New Balance", Slug = "new-balance", LogoUrl = "assets/images/brands/new-balance-logo.svg", DisplayOrder = 4 },
        new() { Id = 6, Name = "Puma", Slug = "puma", LogoUrl = "assets/images/brands/puma-logo.svg", DisplayOrder = 5 },
        new() { Id = 7, Name = "Asics", Slug = "asics", LogoUrl = "assets/images/brands/asics-logo.svg", DisplayOrder = 6 },
        new() { Id = 8, Name = "Reebok", Slug = "reebok", LogoUrl = "assets/images/brands/reebok-logo.svg", DisplayOrder = 7 },
        new() { Id = 9, Name = "Converse", Slug = "converse", LogoUrl = "assets/images/brands/converse-logo.svg", DisplayOrder = 8 },
        new() { Id = 10, Name = "Hoka", Slug = "hoka", LogoUrl = "assets/images/brands/hoka-logo.svg", DisplayOrder = 9 },
        new() { Id = 11, Name = "Salomon", Slug = "salomon", LogoUrl = "assets/images/brands/salomon-logo.svg", DisplayOrder = 10 },
        new() { Id = 12, Name = "Saucony", Slug = "saucony", LogoUrl = "assets/images/brands/saucony-logo.svg", DisplayOrder = 11 },
        new() { Id = 13, Name = "Skechers", Slug = "skechers", LogoUrl = "assets/images/brands/skechers-logo.svg", DisplayOrder = 12 },
        new() { Id = 14, Name = "Brooks", Slug = "brooks", LogoUrl = "assets/images/brands/brooks-logo.svg", DisplayOrder = 13 },
        new() { Id = 15, Name = "Mizuno", Slug = "mizuno", LogoUrl = "assets/images/brands/mizuno-logo.svg", DisplayOrder = 14 },
        new() { Id = 16, Name = "Under Armour", Slug = "under-armour", LogoUrl = "assets/images/brands/under-armour-logo.svg", DisplayOrder = 15 },
        new() { Id = 17, Name = "Fila", Slug = "fila", LogoUrl = "assets/images/brands/fila-logo.svg", DisplayOrder = 16 },
        new() { Id = 18, Name = "Timberland", Slug = "timberland", LogoUrl = "assets/images/brands/timberland-logo.svg", DisplayOrder = 17 },
        new() { Id = 19, Name = "Columbia", Slug = "columbia", LogoUrl = "assets/images/brands/columbia-logo.svg", DisplayOrder = 18 },
        new() { Id = 20, Name = "Lacoste", Slug = "lacoste", LogoUrl = "assets/images/brands/lacoste-logo.svg", DisplayOrder = 19 },
        new() { Id = 21, Name = "Diesel", Slug = "diesel", LogoUrl = "assets/images/brands/diesel-logo.svg", DisplayOrder = 20 },
        new() { Id = 22, Name = "Amiri", Slug = "amiri", LogoUrl = "assets/images/brands/amiri-logo.svg", DisplayOrder = 21 },
        new() { Id = 23, Name = "Balenciaga", Slug = "balenciaga", LogoUrl = "assets/images/brands/balenciaga-logo.svg", DisplayOrder = 22 },
        new() { Id = 24, Name = "Louis Vuitton", Slug = "louis-vuitton", LogoUrl = "assets/images/brands/louis-vuitton-logo.svg", DisplayOrder = 23 },
        new() { Id = 25, Name = "Alexander McQueen", Slug = "alexander-macqueen", LogoUrl = "assets/images/brands/alexander-macqueen-logo.svg", DisplayOrder = 24 },
        new() { Id = 26, Name = "Vico", Slug = "vico", LogoUrl = "assets/images/brands/vico-logo.svg", DisplayOrder = 25 },
    ];

    public static Product[] GetProducts() =>
        Products.Select((p, i) => new Product
        {
            Id = p.Id,
            Name = p.Name,
            BrandId = 1,
            Category = p.Category,
            Usage = p.Usage,
            Price = p.Price,
            Swatch = p.Swatch,
            Badge = p.Badge,
            Rating = p.Rating,
            ReviewCount = p.ReviewCount,
            Description = p.Description,
            DisplayOrder = i,
        }).ToArray();

    public static ProductImage[] GetImages()
    {
        var id = 1;
        return Products.SelectMany(p => Enumerable.Range(0, 3).Select(i => new ProductImage
        {
            Id = id++,
            ProductId = p.Id,
            Url = p.Image,
            SortOrder = i,
        })).ToArray();
    }

    public static ProductSpec[] GetSpecs()
    {
        var id = 1;
        return Products.SelectMany(p => p.Specs.Select((s, i) => new ProductSpec
        {
            Id = id++,
            ProductId = p.Id,
            Key = s.Key,
            Value = s.Value,
            SortOrder = i,
        })).ToArray();
    }

    public static ProductSize[] GetSizes()
    {
        var id = 1;
        // Every product gets modest stock per size, except air-pulse/42 which is
        // deliberately seeded out-of-stock to exercise the disabled-size UI.
        return Products.SelectMany(p => p.Sizes.Select(size => new ProductSize
        {
            Id = id++,
            ProductId = p.Id,
            Size = size,
            StockQty = p.Id == "air-pulse" && size == 42 ? 0 : 4 + (size % 6) * 2,
        })).ToArray();
    }

    private sealed record UserSeed(string Phone, string? FullName, DateTime CreatedAt);

    private static readonly UserSeed[] Users =
    [
        new("09121234567", "علی محمدی", new DateTime(2026, 6, 12, 9, 0, 0, DateTimeKind.Utc)),
        new("09123456789", "سارا احمدی", new DateTime(2026, 6, 18, 11, 30, 0, DateTimeKind.Utc)),
        new("09351112233", "محمد رضایی", new DateTime(2026, 7, 2, 14, 15, 0, DateTimeKind.Utc)),
        new("09199887766", "نگار کریمی", new DateTime(2026, 7, 10, 8, 45, 0, DateTimeKind.Utc)),
        new("09368765432", null, new DateTime(2026, 7, 19, 20, 5, 0, DateTimeKind.Utc)),
        new("09127654321", "امیر حسینی", new DateTime(2026, 7, 25, 16, 40, 0, DateTimeKind.Utc)),
    ];

    public static User[] GetDemoUsers() =>
        Users.Select(u => new User
        {
            PhoneNumber = u.Phone,
            FullName = u.FullName,
            CreatedAt = u.CreatedAt,
        }).ToArray();

    private sealed record OrderItemSeed(string ProductId, int Size, int Qty);

    private sealed record OrderSeed(
        string OrderNumber, Guid CartId, DateTime CreatedAt, OrderStatus Status, OrderItemSeed[] Items);

    private static readonly OrderSeed[] Orders =
    [
        new("OT-10231", Guid.Parse("6f2a1b10-0001-4a10-8000-000000000001"), new DateTime(2026, 7, 21, 10, 15, 0, DateTimeKind.Utc),
            OrderStatus.Delivered, [new("air-pulse", 42, 1)]),

        new("OT-10232", Guid.Parse("6f2a1b10-0002-4a10-8000-000000000002"), new DateTime(2026, 7, 22, 14, 20, 0, DateTimeKind.Utc),
            OrderStatus.Delivered, [new("aria-knit", 38, 1), new("mini-dash", 30, 2)]),

        new("OT-10233", Guid.Parse("6f2a1b10-0003-4a10-8000-000000000003"), new DateTime(2026, 7, 23, 9, 5, 0, DateTimeKind.Utc),
            OrderStatus.Delivered, [new("highjump-x", 43, 1)]),

        new("OT-10234", Guid.Parse("6f2a1b10-0004-4a10-8000-000000000004"), new DateTime(2026, 7, 24, 16, 40, 0, DateTimeKind.Utc),
            OrderStatus.Shipped, [new("street-low", 41, 2)]),

        new("OT-10235", Guid.Parse("6f2a1b10-0005-4a10-8000-000000000005"), new DateTime(2026, 7, 26, 11, 0, 0, DateTimeKind.Utc),
            OrderStatus.Delivered, [new("overtime-06", 42, 1)]),

        new("OT-10236", Guid.Parse("6f2a1b10-0006-4a10-8000-000000000006"), new DateTime(2026, 7, 27, 13, 30, 0, DateTimeKind.Utc),
            OrderStatus.Processing, [new("air-pulse", 44, 1), new("street-low", 40, 1)]),

        new("OT-10237", Guid.Parse("6f2a1b10-0007-4a10-8000-000000000007"), new DateTime(2026, 7, 29, 8, 50, 0, DateTimeKind.Utc),
            OrderStatus.Delivered, [new("mini-dash", 32, 1)]),

        new("OT-10238", Guid.Parse("6f2a1b10-0008-4a10-8000-000000000008"), new DateTime(2026, 7, 30, 19, 10, 0, DateTimeKind.Utc),
            OrderStatus.Pending, [new("highjump-x", 44, 1), new("aria-knit", 39, 1)]),

        new("OT-10239", Guid.Parse("6f2a1b10-0009-4a10-8000-000000000009"), new DateTime(2026, 8, 1, 12, 25, 0, DateTimeKind.Utc),
            OrderStatus.Cancelled, [new("overtime-06", 41, 1)]),

        new("OT-10240", Guid.Parse("6f2a1b10-0010-4a10-8000-000000000010"), new DateTime(2026, 8, 2, 15, 45, 0, DateTimeKind.Utc),
            OrderStatus.Pending, [new("air-pulse", 43, 2), new("street-low", 42, 1)]),
    ];

    private static long PriceOf(string productId) => Products.First(p => p.Id == productId).Price;
    private static string NameOf(string productId) => Products.First(p => p.Id == productId).Name;

    public static Order[] GetDemoOrders() =>
        Orders.Select(o => new Order
        {
            OrderNumber = o.OrderNumber,
            CartId = o.CartId,
            CreatedAt = o.CreatedAt,
            Status = o.Status,
            Total = o.Items.Sum(it => it.Qty * PriceOf(it.ProductId)),
            Items = o.Items.Select(it => new OrderItem
            {
                ProductId = it.ProductId,
                ProductName = NameOf(it.ProductId),
                Size = it.Size,
                Qty = it.Qty,
                UnitPrice = PriceOf(it.ProductId),
            }).ToList(),
        }).ToArray();
}
