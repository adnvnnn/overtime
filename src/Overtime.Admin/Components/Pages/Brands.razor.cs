using Overtime.Data.Entities;

namespace Overtime.Admin.Components.Pages;

public partial class Brands
{
    private static readonly string[] KnownLogoFiles =
    [
        "assets/images/brands/nike-logo.svg",
        "assets/images/brands/adidas-logo.svg",
        "assets/images/brands/airjordan-logo.svg",
        "assets/images/brands/new-balance-logo.svg",
        "assets/images/brands/puma-logo.svg",
        "assets/images/brands/asics-logo.svg",
        "assets/images/brands/reebok-logo.svg",
        "assets/images/brands/converse-logo.svg",
        "assets/images/brands/hoka-logo.svg",
        "assets/images/brands/salomon-logo.svg",
        "assets/images/brands/saucony-logo.svg",
        "assets/images/brands/skechers-logo.svg",
        "assets/images/brands/brooks-logo.svg",
        "assets/images/brands/mizuno-logo.svg",
        "assets/images/brands/under-armour-logo.svg",
        "assets/images/brands/fila-logo.svg",
        "assets/images/brands/timberland-logo.svg",
        "assets/images/brands/columbia-logo.svg",
        "assets/images/brands/lacoste-logo.svg",
        "assets/images/brands/diesel-logo.svg",
        "assets/images/brands/amiri-logo.svg",
        "assets/images/brands/balenciaga-logo.svg",
        "assets/images/brands/louis-vuitton-logo.svg",
        "assets/images/brands/alexander-macqueen-logo.svg",
        "assets/images/brands/vico-logo.svg",
    ];

    private List<Brand> _brands = [];
    private bool _showForm;
    private Brand _formModel = new();
    private string? _error;
    private int? _confirmDeleteId;

    protected override async Task OnInitializedAsync() => await Refresh();

    private async Task Refresh() => _brands = await BrandService.GetAllAsync();

    private void ShowAddForm()
    {
        _formModel = new();
        _showForm = true;
        _error = null;
    }

    private void ShowEditForm(Brand brand)
    {
        _formModel = new Brand
        {
            Id = brand.Id,
            Name = brand.Name,
            Slug = brand.Slug,
            LogoUrl = brand.LogoUrl,
            DisplayOrder = brand.DisplayOrder,
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
        if (string.IsNullOrWhiteSpace(_formModel.Name))
        {
            _error = "نام برند را وارد کنید.";
            return;
        }

        var (ok, error) = _formModel.Id == 0
            ? await BrandService.CreateAsync(_formModel)
            : await BrandService.UpdateAsync(_formModel);

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
        var (ok, error) = await BrandService.DeleteAsync(id);
        _confirmDeleteId = null;
        if (!ok)
        {
            _error = error;
            return;
        }
        await Refresh();
    }
}
