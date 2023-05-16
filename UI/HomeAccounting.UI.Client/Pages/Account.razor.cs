using System.Text.RegularExpressions;
using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using HomeAccounting.UI.Shared.Dialogs;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using MudBlazor;

namespace HomeAccounting.UI.Client.Pages;

public partial class Account
{
    private readonly CancellationTokenSource _cts = new();
    private bool _isSuccessSubmit = true;
    private UserView? _user;
    private string? _monobankToken;
    private string? _initialMonobankToken;
    private string? _oldPassword;
    private string? _newPassword;
    private string? _confirmNewPassword;
    private bool _processingMonobankToken;
    private bool _processingChangePassword;

    private MudForm _monobankTokenForm = null!;
    private MudForm _changePasswordForm = null!;

    [Inject]
    private IDialogService DialogService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IAuthService AuthService { get; set; } = null!;

    [Inject]
    private IUserService UserService { get; set; } = null!;

    [Inject]
    private NavigationManager NavigationManager { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    protected override async Task OnInitializedAsync()
    {
        HttpClient.OnValidationError += OnValidationError;
        HttpClient.OnError += OnError;

        _user = await AuthService.GetCurrentUserAsync(_cts.Token);
        _monobankToken = await UserService.GetMonobankTokenAsync(_cts.Token);
    }

    private void OnValidationError(ApiErrorResult? _)
    {
        _isSuccessSubmit = false;
    }

    private void OnError(Exception _)
    {
        _isSuccessSubmit = false;
    }

    private static string? ValidateMonobankTokenInput(
        string? value
    ) => string.IsNullOrWhiteSpace(value) ? "Monobank Token is required" : null;

    private static string? ValidateOldPasswordInput(
        string? value
    ) => string.IsNullOrWhiteSpace(value) ? "Old Password is required" : null;

    private string? ValidateNewPasswordInput(
        string? value
    )
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
                            ? value != _oldPassword
                                ? null
                                : "New Password must be different from Old Password"
                            : "New Password max length is 32 characters"
                        : "New Password must be at least 8 characters long"
                    : "New Password must have at least one digit"
                : "New Password must have at least one lowercase letter"
            : "New Password must have at least one uppercase letter";
    }

    private string? ValidateConfirmNewPasswordInput(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "New Password confirmation is required";
        }

        return _newPassword != value ? "Passwords do not match" : null;
    }

    private async Task SubmitMonobankTokenAsync(
        CancellationToken cancellationToken = default
    )
    {
        _processingMonobankToken = true;

        await _monobankTokenForm.Validate();

        if (!_isSuccessSubmit)
        {
            _processingMonobankToken = false;

            Snackbar.Add(_monobankTokenForm.Errors.FirstOrDefault(), Severity.Error);

            return;
        }

        await UserService.SetMonobankTokenAsync(
            new SetMonobankTokenModel
            {
                Token = _monobankToken
            },
            cancellationToken
        );

        if (_isSuccessSubmit)
        {
            Snackbar.Add(
                _initialMonobankToken is null
                    ? "Successfully Added Monobank Token"
                    : "Successfully Changed Monobank Token",
                Severity.Success);

            _initialMonobankToken = _monobankToken;
        }

        _processingMonobankToken = false;
    }

    private async Task ChangePasswordAsync(
        CancellationToken cancellationToken = default
    )
    {
        _processingChangePassword = true;

        await _changePasswordForm.Validate();

        if (!_isSuccessSubmit)
        {
            _processingChangePassword = false;

            Snackbar.Add(_changePasswordForm.Errors.FirstOrDefault(), Severity.Error);

            return;
        }

        await UserService.ChangePasswordAsync(
            new ChangePasswordModel
            {
                ConfirmNewPassword = _confirmNewPassword ?? string.Empty,
                OldPassword = _oldPassword ?? string.Empty,
                NewPassword = _newPassword ?? string.Empty
            },
            cancellationToken
        );

        if (_isSuccessSubmit)
        {
            Snackbar.Add("Successfully Changed Password", Severity.Success);
        }

        _processingChangePassword = false;
    }

    private async Task UploadFilesAsync(IBrowserFile file)
    {
        var res = await DialogService.ShowAsync<AvatarCropDialog>(
            "Crop avatar",
            new DialogParameters
            {
                { "RawImage", file }
            },
            new DialogOptions
            {
                CloseButton = true,
                MaxWidth = MaxWidth.Medium,
                FullWidth = true,
                CloseOnEscapeKey = false
            }
        );

        var result = await res.Result;

        if (!result.Canceled)
        {
            Snackbar.Add("Successfully Changed Avatar", Severity.Success);
            NavigationManager.NavigateTo(NavigationManager.Uri, true);
        }
    }
}