namespace HomeAccounting.Domain.Services.Abstraction;

public interface IValidationService
{
    public Task<bool> IsUserExistsAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsUserExistsAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public bool IsUserIsCurrentUser(
        Guid id
    );

    public Task<bool> IsUserWithVerificationCodeExistsAsync(
        Guid verificationCode,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsInvitedUserExistAsync(
        Guid invitationToken,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsCurrentUserPasswordCorrectAsync(
        string password,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsEmailUniqueAsync(
        string email,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsSpendingExistAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsSpendingBelongsToCurrentUserAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsIncomingExistAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );

    public Task<bool> IsIncomingBelongsToCurrentUserAsync(
        Guid id,
        CancellationToken cancellationToken = default
    );
}