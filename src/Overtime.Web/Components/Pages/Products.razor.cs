using System.Globalization;
using Microsoft.AspNetCore.Components;
using Overtime.Data.Entities;
using Overtime.Web.Services;

namespace Overtime.Web.Components.Pages;

public partial class Products
{
    [SupplyParameterFromQuery(Name = "cat")]
    public string? CatQuery { get; set; }

    [SupplyParameterFromQuery(Name = "brand")]
    public string? BrandQuery { get; set; }

    private (string Value, string Label)[] _tabs = [];

    private string _category = "all";
    private string _query = "";
    private string _sort = "default";
    private long? _minPrice;
    private long? _maxPrice;
    private bool _inStockOnly;
    private (long Min, long Max) _priceBounds;
    private List<Product> _products = [];
    private int _totalCount;
    private string _countText = "";
    private string? _brandSlug;
    private List<Brand> _allBrands = [];

    private string MinPercent => FormatPercent(_minPrice ?? _priceBounds.Min);
    private string MaxPercent => FormatPercent(_maxPrice ?? _priceBounds.Max);

    private string FormatPercent(long value)
    {
        if (_priceBounds.Max <= _priceBounds.Min)
            return "0%";

        var pct = (double)(value - _priceBounds.Min) / (_priceBounds.Max - _priceBounds.Min) * 100;
        return pct.ToString("0.##", CultureInfo.InvariantCulture) + "%";
    }

    protected override async Task OnInitializedAsync()
    {
        var categories = await CategoryService.GetAllAsync();
        _tabs = [("all", Localizer["Products_TabAll"].Value), .. categories.Select(c => (c.Slug, c.Name))];

        _totalCount = await ProductService.CountAllAsync();
        _priceBounds = await ProductService.GetPriceBoundsAsync();
        _allBrands = await BrandService.GetAllAsync();

        if (!string.IsNullOrEmpty(CatQuery) && _tabs.Any(t => t.Value == CatQuery))
            _category = CatQuery;

        if (!string.IsNullOrEmpty(BrandQuery) && _allBrands.Any(b => b.Slug == BrandQuery))
            _brandSlug = BrandQuery;

        await Refresh();
    }

    private async Task Refresh()
    {
        _products = await ProductService.GetCatalogAsync(_category, _query, _sort, _minPrice, _maxPrice, _inStockOnly, _brandSlug);
        _countText = Localizer["Products_CountText", _products.Count.ToFa(), _totalCount.ToFa()].Value;
    }

    private async Task OnBrandChange(ChangeEventArgs e)
    {
        var value = e.Value?.ToString();
        _brandSlug = string.IsNullOrEmpty(value) ? null : value;
        await Refresh();
    }

    private async Task OnSearchInput(ChangeEventArgs e)
    {
        _query = e.Value?.ToString() ?? "";
        await Refresh();
    }

    private async Task OnSortChange(ChangeEventArgs e)
    {
        _sort = e.Value?.ToString() ?? "default";
        await Refresh();
    }

    private async Task SetCategory(string cat)
    {
        _category = cat;
        await Refresh();
    }

    private void OnMinRangeInput(ChangeEventArgs e)
    {
        if (long.TryParse(e.Value?.ToString(), out var value))
            _minPrice = Math.Min(value, _maxPrice ?? _priceBounds.Max);
    }

    private void OnMaxRangeInput(ChangeEventArgs e)
    {
        if (long.TryParse(e.Value?.ToString(), out var value))
            _maxPrice = Math.Max(value, _minPrice ?? _priceBounds.Min);
    }

    private async Task OnPriceRangeCommit() => await Refresh();

    private async Task OnInStockChange(ChangeEventArgs e)
    {
        _inStockOnly = e.Value is bool value && value;
        await Refresh();
    }

    private async Task ResetFilters()
    {
        _category = "all";
        _query = "";
        _sort = "default";
        _minPrice = null;
        _maxPrice = null;
        _inStockOnly = false;
        _brandSlug = null;
        await Refresh();
    }
}
