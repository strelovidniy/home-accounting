namespace HomeAccounting.UI.Client.Pages;

public partial class Deposits
{
    private readonly CancellationTokenSource _cts = new();

    private bool _isPageLoading = true;

    protected override async Task OnInitializedAsync()
    {
        _isPageLoading = true;

        _isPageLoading = false;
    }
}