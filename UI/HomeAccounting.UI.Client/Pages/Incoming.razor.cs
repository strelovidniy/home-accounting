using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OData.QueryBuilder.Builders;

namespace HomeAccounting.UI.Client.Pages;

public partial class Incoming : IDisposable
{
    private readonly CancellationTokenSource _cts = new();

    private readonly DialogOptions _dialogOptions = new()
    {
        CloseButton = true,
        MaxWidth = MaxWidth.Medium,
        FullWidth = true,
        DisableBackdropClick = true
    };

    private bool _isPageLoading = true;

    private MudTable<IncomingView> _table = null!;

    private bool _isLoading;

    private List<IncomingView> _incomings = new();

    private string _searchString = string.Empty;

    private UserView _currentUser = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IIncomingService IncomingService { get; set; } = null!;

    [Inject]
    private IAuthService AuthService { get; set; } = null!;

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    protected override async Task OnInitializedAsync()
    {
        _currentUser = await AuthService.GetCurrentUserAsync(_cts.Token);

        _isPageLoading = false;
    }

    private async Task AddIncomingDialogAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddIncomingDialog.CurrentUser), _currentUser }
        };

        var dialog = await DialogService.ShowAsync<AddIncomingDialog>("Add Incoming", parameters, _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadServerData();
            Snackbar.Add("Incoming created successfully.", Severity.Success);
        }
    }

    private async Task UpdateIncomingDialogAsync(IncomingView incoming)
    {
        var parameters = new DialogParameters
        {
            { nameof(UpdateIncomingDialog.SelectedIncoming), incoming }
        };

        var dialog = await DialogService.ShowAsync<UpdateIncomingDialog>(
            "Update Incoming",
            parameters,
            new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.ExtraExtraLarge,
                FullWidth = true,
                DisableBackdropClick = true
            });

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadServerData();
            Snackbar.Add("Incoming updated successfully.", Severity.Success);
        }
    }

    private async Task DeleteIncomingAsync(
        IncomingView incoming,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = new DialogParameters
        {
            ["Text"] = "Are you sure you want to delete this incoming?"
        };

        var dialog = await DialogService.ShowAsync<ConfirmDialog>("Confirmation", parameters, _dialogOptions);
        var result = await dialog.Result;

        if (!result.Canceled)
        {
            var isSuccess = true;

            void OnError(Exception _)
            {
                isSuccess = false;
            }

            void OnValidationError(ApiErrorResult? _)
            {
                isSuccess = false;
            }

            HttpClient.OnError += OnError;
            HttpClient.OnValidationError += OnValidationError;

            await IncomingService.DeleteIncomingAsync(incoming.Id, cancellationToken);

            if (!isSuccess)
            {
                HttpClient.OnError -= OnError;
                HttpClient.OnValidationError -= OnValidationError;

                return;
            }

            await _table.ReloadServerData();
            Snackbar.Add("Incoming deleted successfully.", Severity.Success);

            HttpClient.OnError -= OnError;
            HttpClient.OnValidationError -= OnValidationError;
        }
    }

    private async Task<TableData<IncomingView>> ServerReloadAsync(TableState state)
    {
        _isLoading = true;

        var builder = new ODataQueryBuilder("api/odata")
            .For<IncomingView>("incomings")
            .ByList()
            .Top(state.PageSize)
            .Skip(state.PageSize * state.Page)
            .Count();

        builder = state.SortDirection switch
        {
            SortDirection.None => builder,
            SortDirection.Ascending => state.SortLabel switch
            {
                "Description" => builder.OrderBy(o => o.Description),
                "IncomingDate" => builder.OrderBy(o => o.IncomingDate),
                "Amount" => builder.OrderBy(o => o.Amount),
                "IncomingUpdatedAt" => builder.OrderBy(o => o.IncomingUpdatedAt),
                _ => builder
            },
            SortDirection.Descending => state.SortLabel switch
            {
                "Description" => builder.OrderByDescending(o => o.Description),
                "IncomingDate" => builder.OrderByDescending(o => o.IncomingDate),
                "Amount" => builder.OrderByDescending(o => o.Amount),
                "IncomingUpdatedAt" => builder.OrderByDescending(o => o.IncomingUpdatedAt),
                _ => builder
            },
            _ => builder
        };

        if (!string.IsNullOrWhiteSpace(_searchString))
        {
            builder = builder.Filter(
                (role, function) => function.Contains(role.Description, _searchString)
            );
        }

        var oDataResult = await HttpClient.GetFromOdataAsync(builder);

        _incomings = oDataResult?.Value ?? _incomings;

        _isLoading = false;

        return new TableData<IncomingView>
        {
            Items = _incomings,
            TotalItems = oDataResult?.Count ?? 0
        };
    }

    private void ReleaseUnmanagedResources()
    {
        // TODO release unmanaged resources here
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();

        if (!disposing)
        {
            return;
        }

        _cts.Dispose();
        Snackbar.Dispose();
    }

    ~Incoming()
    {
        Dispose(false);
    }
}