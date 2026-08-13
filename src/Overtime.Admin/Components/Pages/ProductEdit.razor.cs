using Microsoft.AspNetCore.Components;
using Overtime.Data.Entities;

namespace Overtime.Admin.Components.Pages;

public partial class ProductEdit
{
    [Parameter]
    public int? Id { get; set; }

    private bool IsNew => Id is null;
    private bool _loaded;
    private string? _error;
    private Product _model = new();
    private List<Brand> _brands = [];
    private List<Category> _categories = [];
    private List<Usage> _usages = [];
    private HashSet<int> _selectedCategoryIds = [];
    private HashSet<int> _selectedUsageIds = [];
    private List<SizeRow> _sizeRows = [];
    private int _nextRowKey;

    private sealed class SizeRow
    {
        public int Key { get; set; }
        public int Size { get; set; }
        public int StockQty { get; set; }
    }

    protected override async Task OnInitializedAsync()
    {
        _brands = await BrandService.GetAllAsync();
        _categories = await CategoryService.GetAllAsync();
        _usages = await UsageService.GetAllAsync();

        if (!IsNew)
        {
            var existing = await ProductService.GetByIdAsync(Id!.Value);
            if (existing is null)
            {
                _error = "محصول یافت نشد.";
                _loaded = true;
                return;
            }
            _model = existing;
            _selectedCategoryIds = existing.Categories.Select(c => c.Id).ToHashSet();
            _selectedUsageIds = existing.Usages.Select(u => u.Id).ToHashSet();
        }

        _sizeRows = _model.Sizes
            .OrderBy(s => s.Size)
            .Select(s => new SizeRow { Key = _nextRowKey++, Size = s.Size, StockQty = s.StockQty })
            .ToList();

        _loaded = true;
    }

    private void AddSizeRow() => _sizeRows.Add(new SizeRow { Key = _nextRowKey++ });

    private void RemoveSize(SizeRow row) => _sizeRows.Remove(row);

    private void OnSizeValueChanged(SizeRow row, ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var size))
            row.Size = Math.Max(0, size);
    }

    private void OnStockChanged(SizeRow row, ChangeEventArgs e)
    {
        if (int.TryParse(e.Value?.ToString(), out var qty))
            row.StockQty = Math.Max(0, qty);
    }

    private void ToggleCategory(int categoryId, ChangeEventArgs e)
    {
        if (e.Value is true) _selectedCategoryIds.Add(categoryId);
        else _selectedCategoryIds.Remove(categoryId);
    }

    private void ToggleUsage(int usageId, ChangeEventArgs e)
    {
        if (e.Value is true) _selectedUsageIds.Add(usageId);
        else _selectedUsageIds.Remove(usageId);
    }

    private async Task SaveAsync()
    {
        if (string.IsNullOrWhiteSpace(_model.Name) || _model.BrandId <= 0 ||
            _selectedCategoryIds.Count == 0 || _model.Price <= 0)
        {
            _error = "نام، برند، دسته‌بندی و قیمت محصول را به‌درستی وارد کنید.";
            return;
        }

        var enteredSizes = _sizeRows.Where(r => r.Size > 0).ToList();

        if (enteredSizes.Count == 0)
        {
            _error = "حداقل یک سایز اضافه کنید.";
            return;
        }

        if (enteredSizes.Select(r => r.Size).Distinct().Count() != enteredSizes.Count)
        {
            _error = "سایزهای تکراری وارد شده است.";
            return;
        }

        var chosenSizes = enteredSizes
            .Select(r => new ProductSize { Size = r.Size, StockQty = r.StockQty })
            .ToList();

        var categoryIds = _selectedCategoryIds.ToList();
        var usageIds = _selectedUsageIds.ToList();

        if (IsNew)
        {
            _model.Sizes = chosenSizes;
            await ProductService.CreateAsync(_model, categoryIds, usageIds);
        }
        else
        {
            await ProductService.UpdateAsync(_model, categoryIds, usageIds);
            await ProductService.UpdateSizesAsync(_model.Id, chosenSizes);
        }
        NavigationManager.NavigateTo("/products");
    }
}
