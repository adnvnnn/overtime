using Overtime.Data.Entities;

namespace Overtime.Admin.Components.Pages;

public partial class Banners
{
    private static readonly string[] KnownImageFiles =
    [
        "assets/images/products/product-1.jpg",
        "assets/images/products/product-2.jpg",
        "assets/images/products/product-3.jpg",
        "assets/images/products/product-4.jpg",
        "assets/images/products/product-5.jpg",
        "assets/images/products/product-6.jpg",
        "assets/images/products/product-7.jpg",
        "assets/images/products/product-8.jpg",
        "assets/images/products/product-9.jpg",
        "assets/images/products/product-12.jpg",
        "assets/images/products/product-91.jpg",
    ];

    private List<Banner> _banners = [];
    private bool _showForm;
    private Banner _formModel = new();
    private string? _error;
    private int? _confirmDeleteId;

    protected override async Task OnInitializedAsync() => await Refresh();

    private async Task Refresh() => _banners = await BannerService.GetAllAsync();

    private static string KindLabel(BannerKind kind) => kind switch
    {
        BannerKind.Hero => "هیرو",
        BannerKind.Promo => "بنر تبلیغاتی",
        _ => kind.ToString(),
    };

    private void ShowAddForm()
    {
        _formModel = new() { IsActive = true };
        _showForm = true;
        _error = null;
    }

    private void ShowEditForm(Banner banner)
    {
        _formModel = new Banner
        {
            Id = banner.Id,
            Kind = banner.Kind,
            ImageUrl = banner.ImageUrl,
            Badge = banner.Badge,
            Title = banner.Title,
            Price = banner.Price,
            LinkUrl = banner.LinkUrl,
            DisplayOrder = banner.DisplayOrder,
            IsActive = banner.IsActive,
        };
        _showForm = true;
        _error = null;
    }

    private void CancelForm()
    {
        _showForm = false;
        _error = null;
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_formModel.ImageUrl))
        {
            _error = "آدرس تصویر را وارد کنید.";
            return;
        }

        var (ok, error) = _formModel.Id == 0
            ? await BannerService.CreateAsync(_formModel)
            : await BannerService.UpdateAsync(_formModel);

        if (!ok)
        {
            _error = error;
            return;
        }

        _showForm = false;
        _error = null;
        await Refresh();
    }

    private async Task DeleteAsync(int id)
    {
        var (ok, error) = await BannerService.DeleteAsync(id);
        _confirmDeleteId = null;
        if (!ok)
        {
            _error = error;
            return;
        }
        await Refresh();
    }
}
