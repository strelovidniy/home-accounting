using System.Text.RegularExpressions;
using HomeAccounting.Models;
using HomeAccounting.Models.Create;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HomeAccounting.UI.Client.Pages.Auth;

public partial class SignUp : IDisposable
{
    private readonly CreateUserModel _model = new();

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _isSuccessSubmit = true;

    private MudForm _form = null!;

    [Parameter]
    [SupplyParameterFromQuery(Name = "vc")]
    public Guid InvitationToken { get; set; }

    [Inject]
    private IUserService UserService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    public void Dispose()
    {
        ReleaseUnmanagedResources();
        GC.SuppressFinalize(this);
    }


    private void OnValidationError(ApiErrorResult? _)
    {
        _isSuccessSubmit = false;
    }

    private void OnError(Exception _)
    {
        _isSuccessSubmit = false;
    }

    protected override Task OnInitializedAsync()
    {
        HttpClient.OnValidationError += OnValidationError;
        HttpClient.OnError += OnError;

        return Task.CompletedTask;
    }

    private void TogglePasswordVisibility()
    {
        if (_passwordVisibility)
        {
            _passwordVisibility = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput = InputType.Password;
        }
        else
        {
            _passwordVisibility = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput = InputType.Text;
        }
    }

    private static string? ValidatePassword(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Password is required";
        }

        return Regex.IsMatch(
            value,
            @"[A-Z]"
        )
            ? Regex.IsMatch(
                value,
                @"[a-z]"
            )
                ? Regex.IsMatch(
                    value,
                    @"[0-9]"
                )
                    ? value.Length >= 8
                        ? value.Length <= 32
                            ? null
                            : "Password max length is 32 characters"
                        : "Password must be at least 8 characters long"
                    : "Password must have at least one digit"
                : "Password must have at least one lowercase letter"
            : "Password must have at least one uppercase letter";
    }

    private string? ValidateConfirmPassword(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Password confirmation is required";
        }

        return _model.Password != value ? "Passwords do not match" : null;
    }

    private async Task OnSubmitAsync()
    {
        try
        {
            if (!_form.IsValid)
            {
                Snackbar.Add(_form.Errors.FirstOrDefault(), Severity.Error);

                return;
            }

            await UserService.CreateUserAsync(new CreateUserModel
            {
                Password = _model.Password,
                ConfirmPassword = _model.ConfirmPassword,
                FirstName = _model.FirstName,
                LastName = _model.LastName,
                InvitationToken = InvitationToken
            });

            if (_isSuccessSubmit)
            {
                NavigationManager.NavigateTo("/auth/login");
            }
            else
            {
                _isSuccessSubmit = true;
            }
        }
        catch
        {
            // ignored
        }
    }

    private void ReleaseUnmanagedResources()
    {
        HttpClient.OnValidationError -= OnValidationError;
        HttpClient.OnError -= OnError;
    }

    ~SignUp()
    {
        ReleaseUnmanagedResources();
    }
}