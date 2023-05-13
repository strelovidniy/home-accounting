﻿using System.Text.RegularExpressions;
using HomeAccounting.Models;
using HomeAccounting.UI.Domain.Http.HomeAccountingHttpClient;
using HomeAccounting.UI.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace HomeAccounting.UI.Shared.Dialogs;

public partial class ResetPasswordDialog : IDisposable
{
    private string _email = string.Empty;

    private bool _isSuccessSubmit = true;

    private MudForm _form = null!;

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Inject]
    private IUserService UserService { get; set; } = null!;

    [Inject]
    private ISnackbar Snackbar { get; set; } = null!;

    [Inject]
    private IHomeAccountingHttpClient HttpClient { get; set; } = null!;

    public void Dispose()
    {
        Dispose(true);
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

    private void Submit()
    {
        MudDialog.Close();
    }

    private static string? ValidateEmailField(string? value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            return "Email is required";
        }

        return Regex.IsMatch(
            value,
            @"\A(?:[a-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[a-z0-9!#$%&'*+/=?^_`{|}~-]+)*@(?:[a-z0-9](?:[a-z0-9-]*[a-z0-9])?\.)+[a-z0-9](?:[a-z0-9-]*[a-z0-9])?)\Z",
            RegexOptions.IgnoreCase
        )
            ? null
            : "Email format is not valid";
    }

    private async Task OnSubmitAsync()
    {
        await _form.Validate();

        if (!_form.IsValid)
        {
            Snackbar.Add(_form.Errors.FirstOrDefault(), Severity.Error);

            return;
        }

        await UserService.ResetPasswordAsync(new ResetPasswordModel(_email));

        if (_isSuccessSubmit)
        {
            Submit();
        }

        _isSuccessSubmit = true;
    }

    private void ReleaseUnmanagedResources()
    {
        HttpClient.OnValidationError -= OnValidationError;
        HttpClient.OnError -= OnError;
    }

    private void Dispose(bool disposing)
    {
        ReleaseUnmanagedResources();

        if (!disposing)
        {
            return;
        }

        _form.Dispose();
        MudDialog.Dispose();
        Snackbar.Dispose();
    }

    ~ResetPasswordDialog()
    {
        Dispose(false);
    }
}