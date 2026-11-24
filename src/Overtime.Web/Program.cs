using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.EntityFrameworkCore;
using Overtime.Data;
using Overtime.Web.Components;
using Overtime.Web.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorComponents()
    .AddInteractiveServerComponents();

builder.Services.AddDbContextFactory<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

builder.Services.Configure<FooterOptions>(builder.Configuration.GetSection("Footer"));
builder.Services.Configure<TickerOptions>(builder.Configuration.GetSection("Ticker"));
builder.Services.Configure<StoreInfoOptions>(builder.Configuration.GetSection("Store"));

builder.Services.AddLocalization();

builder.Services.AddHttpContextAccessor();
builder.Services.AddScoped<CartContext>();
builder.Services.AddScoped<ProductService>();
builder.Services.AddScoped<BrandService>();
builder.Services.AddScoped<BannerService>();
builder.Services.AddScoped<CategoryService>();
builder.Services.AddScoped<UsageService>();
builder.Services.AddScoped<CartService>();
builder.Services.AddScoped<OrderService>();
builder.Services.AddScoped<AddressService>();
builder.Services.AddScoped<ToastService>();
builder.Services.AddScoped<OtpService>();
builder.Services.AddScoped<ReviewService>();

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.Cookie.Name = "overtime_auth";
        options.LoginPath = "/account/login";
        options.ExpireTimeSpan = TimeSpan.FromDays(30);
        options.SlidingExpiration = true;
    });
builder.Services.AddAuthorization();
builder.Services.AddCascadingAuthenticationState();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<IDbContextFactory<AppDbContext>>().CreateDbContext();
    db.Database.Migrate();
    if (builder.Configuration.GetValue<bool>("DemoData:Enabled"))
        await DemoDataSeeder.SeedIfEmptyAsync(db);
}

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error", createScopeForErrors: true);
}

app.UseStatusCodePagesWithReExecute("/not-found", createScopeForStatusCodePages: true);

app.UseAuthentication();
app.UseAuthorization();
app.UseAntiforgery();

// Ensure every visitor has an anonymous cart id, available to CartContext via HttpContext.Items.
app.Use(async (context, next) =>
{
    if (!Guid.TryParse(context.Request.Cookies[CartCookie.Name], out var cartId))
    {
        cartId = Guid.NewGuid();
        context.Response.Cookies.Append(CartCookie.Name, cartId.ToString(), new CookieOptions
        {
            HttpOnly = true,
            IsEssential = true,
            SameSite = SameSiteMode.Lax,
            Expires = DateTimeOffset.UtcNow.AddYears(1),
        });
    }
    context.Items[CartCookie.Name] = cartId;
    await next();
});

app.MapStaticAssets();
app.MapRazorComponents<App>()
    .AddInteractiveServerRenderMode();

app.MapPost("/account/logout", async (HttpContext context) =>
{
    await context.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
    var returnUrl = context.Request.Form["returnUrl"].FirstOrDefault();
    return Results.LocalRedirect(string.IsNullOrEmpty(returnUrl) ? "/" : returnUrl);
}).RequireAuthorization();

app.Run();
