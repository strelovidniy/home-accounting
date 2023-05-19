using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using MudBlazor;

namespace HomeAccounting.UI.Shared.Dialogs;

public partial class AddMultipleSpendingsDialog : IDisposable
{
    private readonly CancellationTokenSource _cts = new();

    private readonly List<string> _images = new();

    private List<AddSpendingDialog> _models = new();

    private bool _isDialogLoading = true;

    private bool _isSuccessSubmit = true;

    private bool _processing;

    private List<MudForm> _form = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public UserView CurrentUser { get; set; } = null!;

    [Parameter]
    public List<IBrowserFile> Images { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISpendingService SpendingService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IJSRuntime JsRuntime { get; set; } = null!;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void OnError(Exception _)
    {
        _isSuccessSubmit = false;
    }

    private void OnValidationError(ApiErrorResult? _)
    {
        _isSuccessSubmit = false;
    }

    protected override async Task OnInitializedAsync()
    {
        HttpClient.OnError += OnError;
        HttpClient.OnValidationError += OnValidationError;

        foreach (var browserFile in Images)
        {
            var bytes = new byte[browserFile.Size];

            var stream = browserFile.OpenReadStream(10 * 1024 * 1024);

            await stream.ReadAsync(bytes);

            _images.Add(Convert.ToBase64String(bytes));
        }

        _isDialogLoading = false;
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            await LoadDataAsync();
        }
    }

    private async Task LoadDataAsync()
    {
        try
        {
        }
        catch
        {
            //ignored
        }
    }

    private async Task TestJsAsync()
    {
        await JsRuntime.InvokeAsync<dynamic>("testTesseract");
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();

        if (!disposing)
        {
            return;
        }

        _cts.Cancel();
        _cts.Dispose();
        HttpClient.OnError -= OnError;
        HttpClient.OnValidationError -= OnValidationError;
    }

    private void ReleaseUnmanagedResources()
    {
        HttpClient.OnError -= OnError;
        HttpClient.OnValidationError -= OnValidationError;
    }

    ~AddMultipleSpendingsDialog()
    {
        Dispose(false);
    }
}