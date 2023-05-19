using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using OData.QueryBuilder.Builders;

namespace HomeAccounting.UI.Client.Pages;

public partial class Credits : IDisposable
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

    private MudTable<CreditView> _table = null!;

    private bool _isLoading;

    private List<CreditView> _credits = new();

    private string _searchString = string.Empty;

    private UserView _currentUser = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ICreditService CreditService { get; set; } = null!;

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

    private async Task AddCreditsDialogAsync()
    {
        var parameters = new DialogParameters
        {
            { nameof(AddCreditDialog.CurrentUser), _currentUser }
        };

        var dialog = await DialogService.ShowAsync<AddCreditDialog>("Add Credit", parameters, _dialogOptions);

        var result = await dialog.Result;

        if (!result.Canceled)
        {
            await _table.ReloadServerData();
            Snackbar.Add("Credit created successfully.", Severity.Success);
        }
    }

    private async Task UpdateCreditDialogAsync(CreditView Credit)
    {
        var parameters = new DialogParameters
        {
            { nameof(UpdateCreditDialog.SelectedCredit), Credit }
        };

        var dialog = await DialogService.ShowAsync<UpdateCreditDialog>(
            "Update Credit",
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
            Snackbar.Add("Credit updated successfully.", Severity.Success);
        }
    }

    private async Task DeleteCreditAsync(
        CreditView credit,
        CancellationToken cancellationToken = default
    )
    {
        var parameters = new DialogParameters
        {
            ["Text"] = "Are you sure you want to delete this Credit?"
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

            await CreditService.DeleteCreditAsync(credit.Id, cancellationToken);

            if (!isSuccess)
            {
                HttpClient.OnError -= OnError;
                HttpClient.OnValidationError -= OnValidationError;

                return;
            }

            await _table.ReloadServerData();
            Snackbar.Add("Credit deleted successfully.", Severity.Success);

            HttpClient.OnError -= OnError;
            HttpClient.OnValidationError -= OnValidationError;
        }
    }

    private async Task<TableData<CreditView>> ServerReloadAsync(TableState state)
    {
        _isLoading = true;

        var builder = new ODataQueryBuilder("api/odata")
            .For<CreditView>("credits")
            .ByList()
            .Top(state.PageSize)
            .Skip(state.PageSize * state.Page);
        
        builder = state.SortDirection switch
        {
            SortDirection.None => builder,
            SortDirection.Ascending => state.SortLabel switch
            {
                "Description" => builder.OrderBy(o => o.Description),
                "CreditDate" => builder.OrderBy(o => o.CreditDate),
                "Amount" => builder.OrderBy(o => o.Amount),
                "CreditUpdatedAt" => builder.OrderBy(o => o.CreditUpdatedAt),
                _ => builder
            },
            SortDirection.Descending => state.SortLabel switch
            {
                "Description" => builder.OrderByDescending(o => o.Description),
                "CreditDate" => builder.OrderByDescending(o => o.CreditDate),
                "Amount" => builder.OrderByDescending(o => o.Amount),
                "CreditUpdatedAt" => builder.OrderByDescending(o => o.CreditUpdatedAt),
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

        _credits = oDataResult?.Value ?? _credits;

        _isLoading = false;

        return new TableData<CreditView>
        {
            Items = _credits,
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

    ~Credits()
    {
        Dispose(false);
    }
}