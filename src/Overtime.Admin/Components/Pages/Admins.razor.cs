using System.ComponentModel.DataAnnotations;
using System.Security.Claims;
using Overtime.Data.Entities;

namespace Overtime.Admin.Components.Pages;

public partial class Admins
{
    private List<AdminUser> _admins = [];
    private bool _showAddForm;
    private NewAdminModel _newAdmin = new();
    private string? _error;
    private int? _confirmDeleteId;
    private int? _resetId;
    private string _newPassword = "";
    private int _currentAdminId;

    protected override async Task OnInitializedAsync()
    {
        var state = await AuthStateProvider.GetAuthenticationStateAsync();
        _currentAdminId = int.TryParse(state.User.FindFirst(ClaimTypes.NameIdentifier)?.Value, out var id) ? id : 0;
        await Refresh();
    }

    private async Task Refresh() => _admins = await AdminAccountService.GetAllAsync();

    private void ToggleAddForm()
    {
        _showAddForm = !_showAddForm;
        _newAdmin = new();
        _error = null;
    }

    private async Task CreateAsync()
    {
        var (ok, error) = await AdminAccountService.CreateAsync(_newAdmin.Username, _newAdmin.Password, _newAdmin.FullName);
        if (!ok)
        {
            _error = error;
            return;
        }

        _showAddForm = false;
        _error = null;
        await Refresh();
    }

    private void ToggleReset(int id)
    {
        _resetId = _resetId == id ? null : id;
        _newPassword = "";
    }

    private async Task ResetPasswordAsync(int id)
    {
        if (string.IsNullOrWhiteSpace(_newPassword) || _newPassword.Length < 4)
        {
            _error = "رمز عبور باید حداقل ۴ کاراکتر باشد.";
            return;
        }

        await AdminAccountService.ResetPasswordAsync(id, _newPassword);
        _resetId = null;
        _error = null;
    }

    private async Task DeleteAsync(int id)
    {
        var (ok, error) = await AdminAccountService.DeleteAsync(id);
        _confirmDeleteId = null;
        if (!ok)
        {
            _error = error;
            return;
        }
        await Refresh();
    }

    public class NewAdminModel
    {
        [Required(ErrorMessage = "نام کاربری را وارد کنید")]
        public string Username { get; set; } = "";

        public string? FullName { get; set; }

        [Required(ErrorMessage = "رمز عبور را وارد کنید")]
        [MinLength(4, ErrorMessage = "رمز عبور باید حداقل ۴ کاراکتر باشد")]
        public string Password { get; set; } = "";
    }
}
