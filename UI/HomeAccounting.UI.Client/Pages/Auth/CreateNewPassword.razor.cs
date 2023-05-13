using System.Text.RegularExpressions;
using HomeAccounting.Models;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HomeAccounting.UI.Client.Pages.Auth;

public partial class CreateNewPassword
{
    private readonly SetNewPasswordModel _model = new();

    private bool _passwordVisibility;
    private InputType _passwordInput = InputType.Password;
    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private bool _isSuccessSubmit = true;

    private MudForm _form = null!;

    [Parameter]
    [SupplyParameterFromQuery(Name = "vc")]
    public Guid VerificationCode { get; set; }

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
            return "New password is required";
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
                            : "New password max length is 32 characters"
                        : "New password must be at least 8 characters long"
                    : "New password must have at least one digit"
                : "New password must have at least one lowercase letter"
            : "New password must have at least one uppercase letter";
    }

    private string? ValidateConfirmPassword(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "New password confirmation is required";
        }

        return _model.NewPassword != value ? "Passwords do not match" : null;
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

            await UserService.SetNewPasswordAsync(new SetNewPasswordModel
            {
                NewPassword = _model.NewPassword,
                ConfirmNewPassword = _model.ConfirmNewPassword,
                VerificationCode = VerificationCode
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

    ~CreateNewPassword()
    {
        ReleaseUnmanagedResources();
    }
}