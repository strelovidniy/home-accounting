namespace HomeAccounting.Domain.Services.Abstraction;

public interface IMonoApiService
{
    public Task SyncMonoOperationsAsync(
        Guid currentUserId,
        string monoToken,
        string acc = "0",
        CancellationToken cancellationToken = default
    );
}