using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OData.QueryBuilder.Builders;

namespace HomeAccounting.UI.Client.Pages;

public partial class Deposits : IDisposable
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

    private MudTable<DepositView> _table = null!;

    private bool _isLoading;

    private List<DepositView> _deposits = new();

    private string _searchString = string.Empty;

    private UserView _currentUser = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private IDepositService DepositService { get; set; } = null!;

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

    private async Task AddDepositsDialogAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddDepositDialog.CurrentUser), _currentUser }
        };

        var dialog = await DialogService.ShowAsync<AddDepositDialog>("Add Deposit", parameters, _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadServerData();
            Snackbar.Add("Deposit created successfully.", Severity.Success);
        }
    }

    private async Task UpdateDepositDialogAsync(DepositView deposit)
    {
        var parameters = new DialogParameters
        {
            { nameof(UpdateDepositDialog.SelectedDeposit), deposit }
        };

        var dialog = await DialogService.ShowAsync<UpdateDepositDialog>(
            "Update Deposit",
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
            Snackbar.Add("Deposit updated successfully.", Severity.Success);
        }
    }

    private async Task DeleteDepositAsync(
        DepositView deposit,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = new DialogParameters
        {
            ["Text"] = "Are you sure you want to delete this Deposit?"
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

            await DepositService.DeleteDepositAsync(deposit.Id, cancellationToken);

            if (!isSuccess)
            {
                HttpClient.OnError -= OnError;
                HttpClient.OnValidationError -= OnValidationError;

                return;
            }

            await _table.ReloadServerData();
            Snackbar.Add("Deposit deleted successfully.", Severity.Success);

            HttpClient.OnError -= OnError;
            HttpClient.OnValidationError -= OnValidationError;
        }
    }

    private async Task<TableData<DepositView>> ServerReloadAsync(TableState state)
    {
        _isLoading = true;

        var builder = new ODataQueryBuilder("api/odata")
            .For<DepositView>("Deposits")
            .ByList()
            .Top(state.PageSize)
            .Skip(state.PageSize * state.Page);

        builder = state.SortDirection switch
        {
            SortDirection.None => builder,
            SortDirection.Ascending => state.SortLabel switch
            {
                "Description" => builder.OrderBy(o => o.Description),
                "DepositDate" => builder.OrderBy(o => o.DepositDate),
                "Amount" => builder.OrderBy(o => o.Amount),
                "IncomingUpdatedAt" => builder.OrderBy(o => o.DepositUpdatedAt),
                _ => builder
            },
            SortDirection.Descending => state.SortLabel switch
            {
                "Description" => builder.OrderByDescending(o => o.Description),
                "DepositDate" => builder.OrderByDescending(o => o.DepositDate),
                "Amount" => builder.OrderByDescending(o => o.Amount),
                "DepositUpdatedAt" => builder.OrderByDescending(o => o.DepositUpdatedAt),
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

        _deposits = oDataResult?.Value ?? _deposits;

        _isLoading = false;

        return new TableData<DepositView>
        {
            Items = _deposits,
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

    ~Deposits()
    {
        Dispose(false);
    }
}