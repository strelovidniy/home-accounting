using HomeAccounting.Models;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HomeAccounting.UI.Shared.Dialogs;

public partial class UpdateIncomingDialog
{
    private readonly CancellationTokenSource _cts = new();

    private readonly UpdateIncomingModel _model = new();

    private bool _isDialogLoading = true;

    private bool _isSuccessSubmit = true;

    private bool _processing;

    private MudForm _form = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public IncomingView SelectedIncoming { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private IIncomingService IncomingService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

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

    protected override Task OnInitializedAsync()
    {
        HttpClient.OnError += OnError;
        HttpClient.OnValidationError += OnValidationError;

        _model.IncomingId = SelectedIncoming.Id;

        _isDialogLoading = false;

        return Task.CompletedTask;
    }

    private static string? ValidateDescriptionField(string value) =>
        !string.IsNullOrEmpty(value) && value.Length > 250
            ? "Description must be less than 250 characters"
            : null;

    private async Task OnSubmitAsync()
    {
        _processing = true;

        await _form.Validate();

        if (!_form.IsValid)
        {
            _processing = false;

            Snackbar.Add(_form.Errors.FirstOrDefault(), Severity.Error);

            return;
        }

        await IncomingService.UpdateIncomingAsync(_model, _cts.Token);

        if (_isSuccessSubmit)
        {
            MudDialog.Close();

            return;
        }

        _processing = false;

        _isSuccessSubmit = true;
    }

    private void ReleaseUnmanagedResources()
    {
        HttpClient.OnError -= OnError;
        HttpClient.OnValidationError -= OnValidationError;
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
        _form.Dispose();
        MudDialog.Dispose();
        Snackbar.Dispose();
    }

    ~UpdateIncomingDialog()
    {
        Dispose(false);
    }
}