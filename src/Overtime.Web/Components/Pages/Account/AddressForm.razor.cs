using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http;
using Overtime.Web.Resources;

namespace Overtime.Web.Components.Pages.Account;

public partial class AddressForm
{
    [Parameter]
    public int? AddressId { get; set; }

    [CascadingParameter]
    public HttpContext HttpContext { get; set; } = default!;

    [SupplyParameterFromForm]
    public AddressFormModel FormModel { get; set; } = new();

    private int _userId;
    private bool IsNew => AddressId is null;

    protected override async Task OnInitializedAsync()
    {
        var userIdClaim = HttpContext?.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (!int.TryParse(userIdClaim, out _userId))
            NavigationManager.NavigateTo("/account/login");
        else if (!IsNew)
            await LoadAddress();
    }

    private async Task LoadAddress()
    {
        var address = await AddressService.GetAddressAsync(AddressId!.Value, _userId);
        if (address is not null)
        {
            FormModel.RecipientName = address.RecipientName;
            FormModel.PhoneNumber = address.PhoneNumber;
            FormModel.Province = address.Province;
            FormModel.City = address.City;
            FormModel.District = address.District;
            FormModel.Address1 = address.Address1;
            FormModel.Address2 = address.Address2;
            FormModel.PostalCode = address.PostalCode;
            FormModel.IsDefault = address.IsDefault;
        }
        else
            NavigationManager.NavigateTo("/account");
    }

    private async Task SaveAddress()
    {
        if (IsNew)
        {
            await AddressService.AddAddressAsync(_userId, FormModel.RecipientName, FormModel.PhoneNumber,
                FormModel.Province, FormModel.City, FormModel.District, FormModel.Address1,
                FormModel.Address2, FormModel.PostalCode, FormModel.IsDefault);
        }
        else
        {
            await AddressService.UpdateAddressAsync(AddressId!.Value, _userId, FormModel.RecipientName,
                FormModel.PhoneNumber, FormModel.Province, FormModel.City, FormModel.District,
                FormModel.Address1, FormModel.Address2, FormModel.PostalCode, FormModel.IsDefault);
        }

        NavigationManager.NavigateTo("/account");
    }

    private void Cancel() => NavigationManager.NavigateTo("/account");

    public class AddressFormModel
    {
        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_RecipientNameRequired))]
        public string RecipientName { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_PhoneRequired))]
        public string PhoneNumber { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_ProvinceRequired))]
        public string Province { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_CityRequired))]
        public string City { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_DistrictRequired))]
        public string District { get; set; } = "";

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_Address1Required))]
        public string Address1 { get; set; } = "";

        public string? Address2 { get; set; }

        [Required(ErrorMessageResourceType = typeof(AppStrings), ErrorMessageResourceName = nameof(AppStrings.Validation_PostalCodeRequired))]
        public string PostalCode { get; set; } = "";

        public bool IsDefault { get; set; }
    }
}
