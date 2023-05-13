﻿using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Helpers;
using HomeAccounting.Domain.Services.Abstraction;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Services.Realization;

internal class ValidationService : IValidationService
{
    private readonly IRepository<User> _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public ValidationService(
        IRepository<User> userRepository,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
    }

    public Task<bool> IsUserExistsAsync(
        string email,
        CancellationToken cancellationToken = default
    ) => _userRepository
        .Query()
        .AnyAsync(user => user.Email == email, cancellationToken);

    public Task<bool> IsUserExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    ) => _userRepository
        .Query()
        .AnyAsync(user => user.Id == id, cancellationToken);


    public Task<bool> IsUserWithVerificationCodeExistsAsync(
        Guid verificationCode,
        CancellationToken cancellationToken = default
    ) => _userRepository
        .Query()
        .AnyAsync(user => user.VerificationCode == verificationCode, cancellationToken);

    public Task<bool> IsInvitedUserExistAsync(
        Guid invitationToken,
        CancellationToken cancellationToken = default
    ) => _userRepository
        .Query()
        .AnyAsync(user => user.InvitationToken == invitationToken, cancellationToken);

    public Task<bool> IsCurrentUserPasswordCorrectAsync(
        string password,
        CancellationToken cancellationToken = default
    ) => _userRepository
        .Query()
        .AnyAsync(
            user => user.PasswordHash == PasswordHasher.GetHash(password)
                && user.Id == _httpContextAccessor.GetCurrentUserId(),
            cancellationToken
        );

    public async Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default
    ) => !await _userRepository
        .Query()
        .AnyAsync(user => user.Email == email, cancellationToken);
}