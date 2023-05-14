using Blazored.LocalStorage;
using HomeAccounting.Data.Entities;
using HomeAccounting.Models;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HomeAccounting.UI.Shared.Components;

public partial class MainLayout : IDisposable
{
    private User? _currentUser;

    private bool _drawerOpen = true;
    private bool _settingsExpanded = true;
    private bool _adminExpanded = true;

    private bool _isDarkMode;
    private MudThemeProvider _mudThemeProvider = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private IAuthService AuthService { get; set; } = null!;

    [Inject]
    private NavigationManager NavManager { get; set; } = null!;

    [Inject]
    private ILocalStorageService LocalStorageService { get; set; } = null!;

    private char? FirstLetterOfName { get; set; }

    private string FullName { get; set; }

    public bool IsAuth => NavManager.Uri.Contains("/auth/");

    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    private void Init()
    {
        HttpClient.OnForbidden += OnForbidden;
        HttpClient.OnUnauthorized += OnUnauthorized;
        HttpClient.OnInternalServerError += OnInternalServerError;
        HttpClient.OnBadRequest += OnBadRequest;
        HttpClient.OnNotFound += OnNotFound;
        HttpClient.OnValidationError += OnValidationError;
        HttpClient.OnError += OnError;
    }

    public void OnForbidden(HttpResponseMessage response)
    {
        //NavManager.NavigateTo($"auth/login?returnUrl={Uri.EscapeDataString(NavManager.Uri)}");
    }

    public void OnUnauthorized(HttpResponseMessage response)
    {
        LogoutAsync().AndForget();
    }

    public void OnBadRequest(HttpResponseMessage response)
    {
        //NavManager.NavigateTo($"auth/login?returnUrl={Uri.EscapeDataString(NavManager.Uri)}");
    }

    public void OnNotFound(HttpResponseMessage response)
    {
        //NavManager.NavigateTo($"auth/login?returnUrl={Uri.EscapeDataString(NavManager.Uri)}");
    }

    public void OnValidationError(ApiErrorResult? error)
    {
        Snackbar.Add(error?.Message, Severity.Error);
    }

    public void OnInternalServerError(ApiErrorResult? error)
    {
        Snackbar.Add(error?.Message, Severity.Error);
    }

    public void OnError(Exception ex)
    {
        Snackbar.Add(ex.Message, Severity.Error);
    }

    protected override Task OnInitializedAsync()
    {
        Init();

        return Task.CompletedTask;
    }

    private void GetPermissions()
    {
    }

    protected override async Task OnAfterRenderAsync(bool firstRender)
    {
        if (firstRender)
        {
            _isDarkMode = await _mudThemeProvider.GetSystemPreference();
            await _mudThemeProvider.WatchSystemPreference(OnSystemPreferenceChangedAsync);
            await InvokeAsync(StateHasChanged);

            _currentUser = await AuthService.GetCurrentUserAsync();

            if (_currentUser?.FirstName.Length > 0)
            {
                FirstLetterOfName = _currentUser.FirstName[0];
            }

            FullName = $@"{_currentUser?.FirstName} {_currentUser?.LastName}";

            GetPermissions();
        }
    }

    private Task OnSystemPreferenceChangedAsync(bool newValue)
    {
        _isDarkMode = newValue;

        return InvokeAsync(StateHasChanged);
    }


    private async Task LogoutAsync()
    {
        await LocalStorageService.RemoveItemAsync("token");
        NavManager.NavigateTo("auth/login");
    }

    private void ReleaseUnmanagedResources()
    {
        HttpClient.OnForbidden -= OnForbidden;
        HttpClient.OnUnauthorized -= OnUnauthorized;
        HttpClient.OnInternalServerError -= OnInternalServerError;
        HttpClient.OnBadRequest -= OnBadRequest;
        HttpClient.OnNotFound -= OnNotFound;
        HttpClient.OnValidationError -= OnValidationError;
        HttpClient.OnError -= OnError;
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();

        if (disposing)
        {
            Snackbar.Dispose();
        }
    }

    ~MainLayout()
    {
        Dispose(false);
    }
}