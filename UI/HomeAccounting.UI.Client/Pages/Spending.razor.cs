using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OData.QueryBuilder.Builders;

namespace HomeAccounting.UI.Client.Pages;

public partial class Spending : IDisposable
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

    private MudTable<SpendingView> _table = null!;

    private bool _isLoading;

    private List<SpendingView> _spendings = new();

    private string _searchString = string.Empty;

    private UserView _currentUser = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ISpendingService SpendingService { get; set; } = null!;

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

    private async Task AddSpendingDialogAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddSpendingDialog.CurrentUser), _currentUser }
        };

        var dialog = await DialogService.ShowAsync<AddSpendingDialog>("Add Spending", parameters, _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadServerData();
            Snackbar.Add("Spending created successfully.", Severity.Success);
        }
    }

    private async Task UpdateSpendingDialogAsync(SpendingView spending)
    {
        var parameters = new DialogParameters
        {
            { nameof(UpdateSpendingDialog.SelectedSpending), spending }
        };

        var dialog = await DialogService.ShowAsync<UpdateSpendingDialog>(
            "Update Spending",
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
            Snackbar.Add("Spending updated successfully.", Severity.Success);
        }
    }

    private async Task DeleteSpendingAsync(
        SpendingView spending,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = new DialogParameters
        {
            ["Text"] = "Are you sure you want to delete this spending?"
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

            await SpendingService.DeleteSpendingAsync(spending.Id, cancellationToken);

            if (!isSuccess)
            {
                HttpClient.OnError -= OnError;
                HttpClient.OnValidationError -= OnValidationError;

                return;
            }

            await _table.ReloadServerData();
            Snackbar.Add("Spending deleted successfully.", Severity.Success);

            HttpClient.OnError -= OnError;
            HttpClient.OnValidationError -= OnValidationError;
        }
    }

    private async Task<TableData<SpendingView>> ServerReloadAsync(TableState state)
    {
        _isLoading = true;

        var builder = new ODataQueryBuilder("api/odata")
            .For<SpendingView>("spendings")
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
                "SpendingDate" => builder.OrderBy(o => o.SpendingDate),
                "Amount" => builder.OrderBy(o => o.Amount),
                "SpendingUpdatedAt" => builder.OrderBy(o => o.SpendingUpdatedAt),
                _ => builder
            },
            SortDirection.Descending => state.SortLabel switch
            {
                "Description" => builder.OrderByDescending(o => o.Description),
                "SpendingDate" => builder.OrderByDescending(o => o.SpendingDate),
                "Amount" => builder.OrderByDescending(o => o.Amount),
                "SpendingUpdatedAt" => builder.OrderByDescending(o => o.SpendingUpdatedAt),
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

        _spendings = oDataResult?.Value ?? _spendings;

        _isLoading = false;

        return new TableData<SpendingView>
        {
            Items = _spendings,
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

    ~Spending()
    {
        Dispose(false);
    }
}