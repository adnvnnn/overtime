namespace Overtime.Web.Services;

public class ToastService
{
    public event Action<string>? Show;

    public void ShowMessage(string message) => Show?.Invoke(message);
}
