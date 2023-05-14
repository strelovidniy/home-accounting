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

internal class IncomingService : IIncomingService
{
    private readonly IRepository<Incoming> _incomingRepository;
    private readonly IMapper _mapper;
    private readonly IHttpContextAccessor _httpContextAccessor;

    public IncomingService(
        IRepository<Incoming> incomingRepository,
        IMapper mapper,
        IHttpContextAccessor httpContextAccessor
    )
    {
        _incomingRepository = incomingRepository;
        _mapper = mapper;
        _httpContextAccessor = httpContextAccessor;
    }

    public async Task CreateIncomingAsync(
        CreateIncomingModel model,
        CancellationToken cancellationToken = default
    )
    {
        await _incomingRepository
            .AddAsync(
                _mapper.Map<Incoming>(model),
                cancellationToken
            );

        await _incomingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task UpdateIncomingAsync(
        UpdateIncomingModel model,
        CancellationToken cancellationToken = default
    )
    {
        var incoming = await _incomingRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == model.IncomingId,
                cancellationToken
            );

        RuntimeValidator.Assert(incoming is not null, StatusCode.IncomingNotFound);

        if (model.Description != incoming!.Description)
        {
            incoming.Description = model.Description;
            incoming.UpdatedAt = DateTime.UtcNow;
        }

        await _incomingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteIncomingAsync(
        Guid incomingId,
        CancellationToken cancellationToken = default
    )
    {
        var incoming = await _incomingRepository
            .Query()
            .FirstOrDefaultAsync(
                x => x.Id == incomingId,
                cancellationToken
            );

        RuntimeValidator.Assert(incoming is not null, StatusCode.IncomingNotFound);

        RuntimeValidator.Assert(
            incoming!.UserId != _httpContextAccessor.GetCurrentUserId(),
            StatusCode.YouCanOnlyDeleteIncomingForYourself
        );

        _incomingRepository.Delete(incoming!);

        await _incomingRepository.SaveChangesAsync(cancellationToken);
    }

    public async Task<IncomingView> GetIncomingAsync(
        Guid userId,
        Guid incomingId,
        CancellationToken cancellationToken = default
    )
    {
        var incoming = await _incomingRepository
            .Query()
            .Include(x => x.User)
            .FirstOrDefaultAsync(
                x => x.Id == incomingId,
                cancellationToken
            );

        RuntimeValidator.Assert(incoming is not null, StatusCode.IncomingNotFound);

        RuntimeValidator.Assert(incoming!.UserId == userId, StatusCode.YouAreNotAllowedToSeeThisIncoming);

        return _mapper.Map<IncomingView>(incoming!);
    }

    public Task<decimal> GetAverageIncomingAsync(
        Guid userId,
        CancellationToken cancellationToken = default
    ) => _incomingRepository
        .Query()
        .Where(x => x.UserId == userId)
        .Select(x => x.Amount)
        .AverageAsync(cancellationToken);
}