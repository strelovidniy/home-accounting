using MonoBankApi.Models.Responses;

namespace HomeAccounting.Domain.Services.Abstraction;

public interface IMonoApiService
{
    Task<ICollection<StatementResponse>> ReturnStatementAsync(Guid currentUserId, DateTime from, DateTime to,
        CancellationToken ct, string acc = "0");
}