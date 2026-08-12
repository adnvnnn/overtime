using System.Resources;

namespace Overtime.Web.Resources;

// Marker type for IStringLocalizer<AppStrings>, which resolves AppStrings.resx.
// One shared resource file for all Persian UI text in this project, rather than
// one resx per component, so string lookups stay in a single place.
//
// DataAnnotations validation attributes (e.g. [Required(ErrorMessage = ...)])
// need a compile-time constant, so IStringLocalizer's runtime lookup can't be
// used there. For those specific cases, the static properties below read the
// same resx via ErrorMessageResourceType/ErrorMessageResourceName instead.
public sealed class AppStrings
{
    private static readonly ResourceManager ResourceManager =
        new("Overtime.Web.Resources.AppStrings", typeof(AppStrings).Assembly);

    public static string Validation_RecipientNameRequired => ResourceManager.GetString(nameof(Validation_RecipientNameRequired))!;
    public static string Validation_PhoneRequired => ResourceManager.GetString(nameof(Validation_PhoneRequired))!;
    public static string Validation_ProvinceRequired => ResourceManager.GetString(nameof(Validation_ProvinceRequired))!;
    public static string Validation_CityRequired => ResourceManager.GetString(nameof(Validation_CityRequired))!;
    public static string Validation_DistrictRequired => ResourceManager.GetString(nameof(Validation_DistrictRequired))!;
    public static string Validation_Address1Required => ResourceManager.GetString(nameof(Validation_Address1Required))!;
    public static string Validation_PostalCodeRequired => ResourceManager.GetString(nameof(Validation_PostalCodeRequired))!;
    public static string Validation_LoginPhoneRequired => ResourceManager.GetString(nameof(Validation_LoginPhoneRequired))!;
    public static string Validation_OtpCodeRequired => ResourceManager.GetString(nameof(Validation_OtpCodeRequired))!;
    public static string Validation_OtpCodeLength => ResourceManager.GetString(nameof(Validation_OtpCodeLength))!;

    // Read by PersianNumberFormatter.FormatToman, a plain static extension method with no DI access.
    public static string Common_Toman => ResourceManager.GetString(nameof(Common_Toman))!;
}
