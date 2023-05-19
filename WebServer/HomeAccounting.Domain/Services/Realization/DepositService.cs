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
    private readonly IRepository<Deposit> _DepositRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public DepositService(
        IRepository<Deposit> DepositRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _DepositRepository = DepositRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateDepositAsync(
        CreateDepositModel model,
        CancellationToken cancellationToken = default
    )
    {
        await _DepositRepository
            .AddAsync(
                _mapper.Map<Deposit>(model),
                cancellationToken
            );

        await _DepositRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateDepositAsync(
        UpdateDepositModel model,
        CancellationToken cancellationToken = default
    )
    {
        var Deposit = await _DepositRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == model.DepositId,
                cancellationToken
            );

        if (model.Description != Deposit!.Description)
        {
            Deposit.Description = model.Description;
            Deposit.UpdatedAt = DateTime.UtcNow;
        }

        await _DepositRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteDepositAsync(
        Guid DepositId,
        CancellationToken cancellationToken = default
    )
    {
        var Deposit = await _DepositRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == DepositId,
                cancellationToken
            );
        

        _DepositRepository.Delete(Deposit!);

        await _DepositRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<decimal> GetCompoundInterestAsync(Guid depositId, CancellationToken cancellationToken = default)
    {
        var depositModel = await GetDepositAsync(depositId, cancellationToken);
        
        if (depositModel?.CompoundingFrequency == 0)
        {
            throw new ArgumentException();
        }
        
        return depositModel.Amount * (decimal)Math.Pow((1 + depositModel.RateOfInterest / depositModel.CompoundingFrequency),
            (depositModel.CompoundingFrequency * depositModel.NumberOfYears));
    }
    
    private async Task<Deposit?> GetDepositAsync(
        Guid depositId,
        CancellationToken cancellationToken = default
    )
    {
        return await _DepositRepository
            .Query()
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == depositId,
                cancellationToken
            );

    }
}