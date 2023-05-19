using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace HomeAccounting.Domain.Services.Realization;

internal class DepositService : IDepositService
{
    private readonly IRepository<Deposit> _depositRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepositService(
        IRepository<Deposit> depositRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _depositRepository = depositRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateDepositAsync(
        CreateDepositModel model,
        CancellationToken cancellationToken = default
    )
    {
        await _depositRepository
            .AddAsync(
                _mapper.Map<Deposit>(model),
                cancellationToken
            );

        await _depositRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDepositAsync(
        UpdateDepositModel model,
        CancellationToken cancellationToken = default
    )
    {
        var deposit = await _depositRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == model.DepositId,
                cancellationToken
            );

        if (model.Description != deposit!.Description)
        {
            deposit.Description = model.Description;
            deposit.UpdatedAt = DateTime.UtcNow;
        }

        await _depositRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDepositAsync(
        Guid depositId,
        CancellationToken cancellationToken = default
    )
    {
        var deposit = await _depositRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == depositId,
                cancellationToken
            );

        _depositRepository.Delete(deposit!);

        await _depositRepository.SaveChangesAsync(cancellationToken);
    }

    private async Task<Deposit?> GetDepositAsync(
        Guid depositId,
        CancellationToken cancellationToken = default
    )
    {
        return await _depositRepository
            .Query()
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == depositId,
                cancellationToken
            );
    }
}