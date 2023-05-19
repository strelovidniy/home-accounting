using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Services.Abstraction;
using MonoBankApi.Implements.Requests;
using MonoBankApi.Models.Responses;

namespace HomeAccounting.Domain.Services.Realization;

public class MonoApiService : IMonoApiService
{
    private readonly MonoApiClient _client;
    private readonly IRepository<Incoming> _incomingRepository;
    private readonly IRepository<Spending> _spendingRepository;

    public MonoApiService(
        MonoApiClient client,
        IRepository<Incoming> incomingRepository,
        IRepository<Spending> spendingRepository
    )
    {
        _client = client;
        _incomingRepository = incomingRepository;
        _spendingRepository = spendingRepository;
    }

    public async Task SyncMonoOperationsAsync(
        Guid currentUserId,
        string monoToken,
        string acc = "0",
        CancellationToken cancellationToken = default
    )
    {
        if (monoToken is null)
        {
            throw new InvalidOperationException();
        }

        var startFrom = DateTime.UtcNow;

        var operations
            = await _client.HttpGetAsync<ICollection<StatementResponse>>(
                new StatementRequest(startFrom.AddDays(-29), startFrom, acc), monoToken);

        var spendings = new List<Spending>();
        var incomings = new List<Incoming>();

        foreach (var operation in operations)
        {
            var operationDate = new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(operation.Timestamp);

            switch (operation.Amount)
            {
                case 0:
                    continue;
                case > 0:
                    incomings.Add(new Incoming
                    {
                        UserId = currentUserId,
                        Amount = (decimal) operation.Amount / 100,
                        Description = operation.Description,
                        CreatedAt = operationDate
                    });

                    break;
                default:
                    spendings.Add(new Spending
                    {
                        UserId = currentUserId,
                        Amount = (decimal) Math.Abs(operation.Amount) / 100,
                        Description = operation.Description,
                        CreatedAt = operationDate
                    });

                    break;
            }
        }

        if (spendings.Any())
        {
            await _spendingRepository.AddRangeAsync(spendings, cancellationToken);
            await _spendingRepository.SaveChangesAsync(cancellationToken);
        }

        if (incomings.Any())
        {
            await _incomingRepository.AddRangeAsync(incomings, cancellationToken);
            await _incomingRepository.SaveChangesAsync(cancellationToken);
        }
    }
}