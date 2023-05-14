using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Validators.Runtime;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Services.Realization;

internal class SpendingService : ISpendingService
{
    private readonly IMapper _mapper;
    private readonly IRepository<Spending> _spendingRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public SpendingService(
        IRepository<Spending> spendingRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _spendingRepository = spendingRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateSpendingAsync(
        CreateSpendingModel model,
        CancellationToken cancellationToken = default
    )
    {
        await _spendingRepository
            .AddAsync(
                _mapper.Map<Spending>(model),
                cancellationToken
            );

        await _spendingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateSpendingAsync(
        UpdateSpendingModel model,
        CancellationToken cancellationToken = default
    )
    {
        var spending = await _spendingRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == model.SpendingId,
                cancellationToken
            );

        RuntimeValidator.Assert(spending is not null, StatusCode.SpendingNotFound);

        if (model.Description != spending!.Description)
        {
            spending.Description = model.Description;
            spending.UpdatedAt = DateTime.UtcNow;
        }

        await _spendingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteSpendingAsync(
        Guid spendingId,
        CancellationToken cancellationToken = default
    )
    {
        var spending = await _spendingRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == spendingId,
                cancellationToken
            );

        RuntimeValidator.Assert(spending is not null, StatusCode.SpendingNotFound);

        RuntimeValidator.Assert(
            spending!.UserId != _httpContextAccessor.GetCurrentUserId(),
            StatusCode.YouCanOnlyDeleteSpendingForYourself
        );

        _spendingRepository.Delete(spending!);

        await _spendingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<SpendingView> GetSpendingAsync(
        Guid userId,
        Guid spendingId,
        CancellationToken cancellationToken = default
    )
    {
        var spending = await _spendingRepository
            .Query()
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == spendingId,
                cancellationToken
            );

        RuntimeValidator.Assert(spending is not null, StatusCode.SpendingNotFound);

        RuntimeValidator.Assert(spending!.UserId == userId, StatusCode.YouAreNotAllowedToSeeThisSpending);

        return _mapper.Map<SpendingView>(spending!);
    }
}