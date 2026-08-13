using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Overtime.Data.Entities;
using Overtime.Web.Services;

namespace Overtime.Web.Components.Pages;

public partial class ProductDetail
{
    [Parameter]
    public string Slug { get; set; } = "";

    private string? _loadedSlug;
    private Product? Product;
    private List<ProductImage> _images = [];
    private List<Product> _related = [];
    private int _activeIndex;
    private int? _selectedSize;
    private int _qty = 1;
    private bool _shake;

    private ElementReference _sliderRef;
    private DotNetObjectReference<ProductDetail>? _dotNetRef;
    private IJSObjectReference? _galleryHandle;
    private int? _galleryAttachedId;

    private List<ReviewLine> _reviews = [];
    private double _avgRating;
    private int _reviewCount;
    private int? _userId;
    private ReviewLine? _myReview;
    private int _formRating;
    private string _formComment = "";
    private bool _reviewSubmitting;

    [CascadingParameter]
    private HttpContext? HttpContext { get; set; }

    private string LoginUrl => $"/account/login?returnUrl={Uri.EscapeDataString($"/product/{Slug}")}";

    private string CategoryLine => Product is null ? "" : string.Join(" · ",
        new[] { Product.Brand?.Name, string.Join("، ", Product.Categories.Select(c => c.Name)), string.Join("، ", Product.Usages.Select(u => u.Name)) }
            .Where(s => !string.IsNullOrEmpty(s)));

    private string SwatchVar => Product?.Swatch switch
    {
        "accent-2" => "--accent-2",
        "ink" => "--ink",
        _ => "--accent",
    };

    protected override async Task OnParametersSetAsync()
    {
        if (_loadedSlug == Slug)
            return;
        _loadedSlug = Slug;

        var product = await ProductService.GetBySlugAsync(Slug);
        if (product is null)
        {
            Navigator.NavigateTo("/not-found");
            return;
        }

        Product = product;
        _images = product.Images.ToList();
        _activeIndex = 0;
        _selectedSize = null;
        _qty = 1;
        _shake = false;
        _related = await ProductService.GetRelatedAsync(product.Id, 3);

        ExtractUserId();
        await LoadReviews();
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (Product is null || _galleryAttachedId == Product.Id)
            return;

        if (_galleryHandle is not null)
        {
            await _galleryHandle.InvokeVoidAsync("dispose");
            await _galleryHandle.DisposeAsync();
            _galleryHandle = null;
        }

        _dotNetRef ??= DotNetObjectReference.Create(this);
        _galleryHandle = await JS.InvokeAsync<IJSObjectReference>("overtimeGallery.attach", _sliderRef, _dotNetRef);
        _galleryAttachedId = Product.Id;
    }

    private void GoTo(int index)
    {
        if (_images.Count == 0)
            return;
        _activeIndex = ((index % _images.Count) + _images.Count) % _images.Count;
    }

    [JSInvokable]
    public void OnSwipe(int direction)
    {
        GoTo(_activeIndex + direction);
        StateHasChanged();
    }

    private void SelectSize(ProductSize size)
    {
        if (size.StockQty <= 0)
            return;
        _selectedSize = size.Size;
    }

    private void QtyInc() => _qty = Math.Min(10, _qty + 1);
    private void QtyDec() => _qty = Math.Max(1, _qty - 1);

    private async Task AddToCart()
    {
        if (Product is null)
            return;

        if (_selectedSize is null)
        {
            _shake = true;
            StateHasChanged();
            ToastService.ShowMessage(Localizer["PDP_SelectSizeFirst"].Value);
            await Task.Delay(400);
            _shake = false;
            StateHasChanged();
            return;
        }

        var added = await CartService.AddItemAsync(Product.Id, _selectedSize.Value, _qty);
        if (added == 0)
        {
            ToastService.ShowMessage(Localizer["PDP_InsufficientStock"].Value);
            return;
        }

        ToastService.ShowMessage(added < _qty
            ? Localizer["PDP_PartialAdd", added.ToFa(), Product.Name].Value
            : Localizer["ProductCard_AddedToCart", Product.Name].Value);
        CartService.OpenDrawer();
    }

    private void ExtractUserId()
    {
        var claim = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        _userId = int.TryParse(claim, out var id) ? id : null;
    }

    private async Task LoadReviews()
    {
        if (Product is null)
            return;
        _reviews = await ReviewService.GetProductReviewsAsync(Product.Id);
        (_avgRating, _reviewCount) = await ReviewService.GetRatingSummaryAsync(Product.Id);

        if (_userId is not null)
        {
            _myReview = await ReviewService.GetUserReviewAsync(Product.Id, _userId.Value);
            _formRating = _myReview?.Rating ?? 0;
            _formComment = _myReview?.Comment ?? "";
        }
    }

    private void SetFormRating(int rating) => _formRating = rating;

    private async Task SubmitReview()
    {
        if (Product is null || _userId is null || _formRating < 1 || _reviewSubmitting)
            return;

        _reviewSubmitting = true;
        await ReviewService.SubmitReviewAsync(Product.Id, _userId.Value, _formRating,
            string.IsNullOrWhiteSpace(_formComment) ? null : _formComment.Trim());
        await LoadReviews();
        _reviewSubmitting = false;
        ToastService.ShowMessage(Localizer["PDP_ReviewSubmitted"].Value);
    }

    public async ValueTask DisposeAsync()
    {
        if (_galleryHandle is not null)
        {
            try
            {
                await _galleryHandle.InvokeVoidAsync("dispose");
                await _galleryHandle.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
                // Circuit already torn down (e.g. page reload/navigation) — nothing to clean up client-side.
            }
        }
        _dotNetRef?.Dispose();
    }
}
