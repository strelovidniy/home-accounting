﻿using System.Web;
using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums;
using HomeAccounting.Data.Enums.RichEnums;
using HomeAccounting.Domain.Exceptions;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Helpers;
using HomeAccounting.Domain.Models.ViewModels;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Settings.Abstraction;
using HomeAccounting.Domain.Validators.Runtime;
using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Services.Realization;

internal class UserService : IUserService
{
    private readonly IRepository<User> _userRepository;
    private readonly IEmailService _emailService;
    private readonly IUrlSettings _urlSettings;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IMonoApiService _monoApiService;
    private readonly IMapper _mapper;

    public UserService(
        IRepository<User> userRepository,
        IEmailService emailService,
        IUrlSettings urlSettings,
        IHttpContextAccessor httpContextAccessor,
        IMapper mapper,
        IMonoApiService monoApiService
    )
    {
        _userRepository = userRepository;
        _emailService = emailService;
        _urlSettings = urlSettings;
        _httpContextAccessor = httpContextAccessor;
        _mapper = mapper;
        _monoApiService = monoApiService;
    }

    public async Task ResetPasswordAsync(
        ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userRepository
                .Query()
                .FirstOrDefaultAsync(user => user.Email == resetPasswordModel.Email, cancellationToken)
            ?? throw new ApiException(StatusCode.UserNotFound);

        user.VerificationCode = Guid.NewGuid();

        await _userRepository.SaveChangesAsync(cancellationToken);

        var uriBuilder = new UriBuilder(_urlSettings.ResetPasswordUrl);
        var query = HttpUtility.ParseQueryString(uriBuilder.Query);

        query["vc"] = user.VerificationCode.ToString();
        uriBuilder.Query = query.ToString();

        await _emailService.SendEmailAsync(
            resetPasswordModel.Email,
            EmailSubject.ResetPassword,
            new ResetPasswordEmailViewModel(uriBuilder.ToString()),
            cancellationToken: cancellationToken
        );
    }

    public async Task SetNewPasswordAsync(
        SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userRepository
                .Query()
                .FirstOrDefaultAsync(
                    user => user.VerificationCode == setNewPasswordModel.VerificationCode,
                    cancellationToken
                )
            ?? throw new ApiException(StatusCode.UserNotFound);

        user.PasswordHash = PasswordHasher.GetHash(setNewPasswordModel.NewPassword);
        user.VerificationCode = null;

        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task CreateUserAsync(
        CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    )
    {
        await _userRepository.AddAsync(
            _mapper.Map<User>(createUserModel),
            cancellationToken
        );

        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<UserView?> GetUserAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => _mapper.Map<UserView>(await _userRepository
        .Query()
        .FirstOrDefaultAsync(user => user.Id == id, cancellationToken));

    public async Task ChangePasswordAsync(
        ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await _userRepository
                .Query()
                .FirstOrDefaultAsync(user => user.Id == _httpContextAccessor.GetCurrentUserId(), cancellationToken)
            ?? throw new ApiException(StatusCode.Unauthorized);

        RuntimeValidator.Assert(
            currentUser.PasswordHash == PasswordHasher.GetHash(changePasswordModel.OldPassword),
            StatusCode.OldPasswordIncorrect
        );

        RuntimeValidator.Assert(
            changePasswordModel.NewPassword == changePasswordModel.ConfirmNewPassword,
            StatusCode.PasswordsDoNotMatch
        );

        currentUser.PasswordHash = PasswordHasher.GetHash(changePasswordModel.NewPassword);

        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task ChangeAvatarAsync(
        byte[] avatar,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = _userRepository
            .Query()
            .FirstOrDefault(user => user.Id == _httpContextAccessor.GetCurrentUserId());

        RuntimeValidator.Assert(currentUser is not null, StatusCode.Unauthorized);

        var avatarName = $"{currentUser!.Id}.avatar";

        var combine = Path.Combine(Directory.GetCurrentDirectory(),
            "Files",
            "Avatars");

        var filePath = Path.Combine(
            combine,
            avatarName
        );

        if (!Directory.Exists(combine))
        {
            Directory.CreateDirectory(combine);
        }

        await File.WriteAllBytesAsync(filePath, avatar, cancellationToken);

        currentUser.ImageDataUrl = _urlSettings.WebApiUrl + "api/v1/users/avatars/" + avatarName;

        await _userRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task SaveMonobankApiTokenAsync(
        SetMonobankTokenModel model,
        CancellationToken cancellationToken = default
    )
    {
        var currentUser = await _userRepository
                .Query()
                .FirstOrDefaultAsync(user => user.Id == _httpContextAccessor.GetCurrentUserId(), cancellationToken)
            ?? throw new ApiException(StatusCode.Unauthorized);

        currentUser.MonobankToken = model.Token;

        await _userRepository.SaveChangesAsync(cancellationToken);

        await _monoApiService.SyncMonoOperationsAsync(
            currentUser.Id,
            currentUser.MonobankToken!,
            cancellationToken: cancellationToken
        );
    }
}