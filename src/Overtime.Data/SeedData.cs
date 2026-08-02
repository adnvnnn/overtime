using Overtime.Data.Entities;

namespace Overtime.Data;

public static class SeedData
{
    private sealed record ProductSeed(
        string Id, string Name, string Category, long Price, string Swatch, string? Badge,
        double Rating, int ReviewCount, string Description, string Image,
        (string Key, string Value)[] Specs);

    private static readonly ProductSeed[] Products =
    [
        new("air-pulse", "Air Pulse", "مردانه · رانینگ", 2950000, "accent", "پرفروش", 4.8, 124,
            "کتانی رانینگ Air Pulse برای دویدن‌های روزمره و تمرین‌های سبک طراحی شده است. رویه‌ی مش تنفس‌پذیر همراه با میان‌کفش سبک، احساس راحتی را در هر قدم حفظ می‌کند.",
            "assets/images/products/product-1.jpg",
            [("جنس رویه", "مش تنفس‌پذیر"), ("نوع بند", "بندی معمولی"), ("کاربرد", "دویدن و پیاده‌روی"), ("رنگ اصلی", "بنفش")]),

        new("highjump-x", "Highjump X", "مردانه · بسکتبال", 3800000, "accent-2", "جدید", 4.7, 98,
            "Highjump X با میان‌کفش ضربه‌گیر و ساق بلندتر، برای حرکات ناگهانی در زمین بسکتبال طراحی شده و پایداری مچ پا را در فرودها افزایش می‌دهد.",
            "assets/images/products/product-2.jpg",
            [("جنس رویه", "سینتتیک و مش"), ("نوع بند", "بندی بلند"), ("کاربرد", "بسکتبال"), ("رنگ اصلی", "بنفش تیره")]),

        new("street-low", "Street Low", "یونیسکس · روزمره", 2200000, "ink", null, 4.6, 156,
            "یک کتانی روزمره‌ی ساده و همه‌کاره؛ Street Low با طراحی مینیمال و رنگ خنثی، با هر پوششی هم‌نشین می‌شود.",
            "assets/images/products/product-3.jpg",
            [("جنس رویه", "چرم مصنوعی"), ("نوع بند", "بندی معمولی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "مشکی")]),

        new("aria-knit", "Aria Knit", "زنانه · نیت", 2650000, "accent-2", "تخفیف", 4.8, 142,
            "Aria Knit با رویه‌ی نیت کشباف، فرمی نزدیک به فرم پا دارد و برای پوشیدن طولانی‌مدت در طول روز بسیار راحت است.",
            "assets/images/products/product-4.jpg",
            [("جنس رویه", "نیت کشباف"), ("نوع بند", "بندی کشی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "بنفش تیره")]),

        new("mini-dash", "Mini Dash", "بچگانه · روزمره", 1850000, "accent", null, 4.9, 86,
            "Mini Dash مدلی سبک و بادوام برای کودکان است؛ زیره‌ی ضدلغزش و بست چسبی، پوشیدن و درآوردن را برای کودکان آسان می‌کند.",
            "assets/images/products/product-5.jpg",
            [("جنس رویه", "مش نرم"), ("نوع بند", "بست چسبی"), ("کاربرد", "استفاده روزمره"), ("رنگ اصلی", "بنفش")]),

        new("overtime-06", "Overtime №06", "لیمیتد ادیشن", 4500000, "ink", "لیمیتد", 5.0, 41,
            "Overtime №06 نسخه‌ی محدود امضای برند اورتایم است؛ تیراژ کم، جزئیات ویژه و جعبه‌ی اختصاصی این مدل را به یک قطعه‌ی کالکشنی تبدیل می‌کند.",
            "assets/images/products/product-6.jpg",
            [("جنس رویه", "چرم و سوئد"), ("نوع بند", "بندی معمولی"), ("کاربرد", "کالکشنی و روزمره"), ("رنگ اصلی", "مشکی و بنفش")]),
    ];

    private static readonly int[] Sizes = [40, 41, 42, 43, 44, 45];

    public static Product[] GetProducts() =>
        Products.Select((p, i) => new Product
        {
            Id = p.Id,
            Name = p.Name,
            Category = p.Category,
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
        return Products.SelectMany(p => Sizes.Select(size => new ProductSize
        {
            Id = id++,
            ProductId = p.Id,
            Size = size,
            StockQty = p.Id == "air-pulse" && size == 42 ? 0 : 4 + (size % 6) * 2,
        })).ToArray();
    }
}
