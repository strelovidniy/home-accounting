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

internal class CreditService : ICreditService
{
    private readonly IRepository<Credit> _creditRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public CreditService(
        IRepository<Credit> CreditRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _creditRepository = CreditRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateCreditAsync(
        CreateCreditModel model,
        CancellationToken cancellationToken = default
    )
    {
        await _creditRepository
            .AddAsync(
                _mapper.Map<Credit>(model),
                cancellationToken
            );

        await _creditRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateCreditAsync(
        UpdateCreditModel model,
        CancellationToken cancellationToken = default
    )
    {
        var credit = await _creditRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == model.CreditId,
                cancellationToken
            );

        if (model.Description != credit!.Description)
        {
            credit.Description = model.Description;
            credit.UpdatedAt = DateTime.UtcNow;
        }

        await _creditRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteCreditAsync(
        Guid CreditId,
        CancellationToken cancellationToken = default
    )
    {
        var Credit = await _creditRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == CreditId,
                cancellationToken
            );
        

        _creditRepository.Delete(Credit!);

        await _creditRepository.SaveChangesAsync(cancellationToken);
    }
}