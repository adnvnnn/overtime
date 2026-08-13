using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Overtime.Data.Entities;
using Overtime.Web.Services;

namespace Overtime.Web.Components.Pages;

public partial class Home
{
    private StoreInfoOptions Store => StoreInfoAccessor.Value;

    // The ticker track loops by shifting exactly one repeat's width, then
    // snapping back — seamless only while at least one full repeat's worth of
    // extra content is always still on screen. With 5 short phrases that means
    // repeating enough times that (TickerRepeatCount - 1) copies alone are
    // wider than any realistic viewport; 5 repeats keeps ~3-4x a 1920px-wide
    // screen covered. Must stay in sync with the 1/TickerRepeatCount endpoint
    // in the `scroll-rtl` keyframe (index.css).
    private const int TickerRepeatCount = 5;
    private List<string> _tickerItems = [];

    private List<Banner> _heroBanners = [];
    private int _heroIndex = 0;
    private CancellationTokenSource? _heroSlideCts;
    private ElementReference _heroSlidesRef;
    private DotNetObjectReference<Home>? _heroDotNetRef;
    private IJSObjectReference? _heroGalleryHandle;

    private int _inStockCount;
    private List<Banner> _promoBanners = [];
    private List<Brand> _instaBrands = [];

    protected override async Task OnInitializedAsync()
    {
        _tickerItems = TickerOptionsAccessor.Value.Items;

        var inStockCountTask = ProductService.CountInStockAsync();
        var heroBannersTask = BannerService.GetActiveAsync(BannerKind.Hero);
        var promoBannersTask = BannerService.GetActiveAsync(BannerKind.Promo);
        var allBrandsTask = BrandService.GetAllAsync();
        await Task.WhenAll(inStockCountTask, heroBannersTask, promoBannersTask, allBrandsTask);

        _inStockCount = inStockCountTask.Result;
        _heroBanners = heroBannersTask.Result;
        _promoBanners = promoBannersTask.Result;
        _instaBrands = [.. allBrandsTask.Result.OrderBy(_ => Random.Shared.Next()).Take(6)];
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _heroDotNetRef = DotNetObjectReference.Create(this);
            _heroGalleryHandle = await JS.InvokeAsync<IJSObjectReference>("overtimeGallery.attach", _heroSlidesRef, _heroDotNetRef);

            _heroSlideCts = new CancellationTokenSource();
            _ = RunHeroAutoSlideAsync(_heroSlideCts.Token);
        }
    }

    private async Task RunHeroAutoSlideAsync(CancellationToken token)
    {
        try
        {
            while (!token.IsCancellationRequested)
            {
                await Task.Delay(4500, token);
                if (_heroBanners.Count == 0)
                    continue;
                _heroIndex = (_heroIndex + 1) % _heroBanners.Count;
                await InvokeAsync(StateHasChanged);
            }
        }
        catch (TaskCanceledException)
        {
        }
    }

    private void GoToHeroSlide(int index) => _heroIndex = index;

    [JSInvokable]
    public void OnSwipe(int direction)
    {
        if (_heroBanners.Count == 0)
            return;

        // overtimeGallery reports physical (LTR) drag direction; this page is RTL, so invert it.
        _heroIndex = ((_heroIndex - direction) % _heroBanners.Count + _heroBanners.Count) % _heroBanners.Count;
        StateHasChanged();
    }

    public async ValueTask DisposeAsync()
    {
        _heroSlideCts?.Cancel();
        _heroSlideCts?.Dispose();

        if (_heroGalleryHandle is not null)
        {
            try
            {
                await _heroGalleryHandle.InvokeVoidAsync("dispose");
                await _heroGalleryHandle.DisposeAsync();
            }
            catch (JSDisconnectedException)
            {
            }
        }
        _heroDotNetRef?.Dispose();
    }
}
