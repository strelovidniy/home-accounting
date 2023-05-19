﻿using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;

namespace HomeAccounting.Domain.Services.Abstraction;

public interface IDepositService
{
    public Task CreateDepositAsync(
        CreateDepositModel model,
        CancellationToken cancellationToken = default
    );

    public Task UpdateDepositAsync(
        UpdateDepositModel model,
        CancellationToken cancellationToken = default
    );

    public Task DeleteDepositAsync(
        Guid DepositId,
        CancellationToken cancellationToken = default
    );

    Task<decimal> GetCompoundInterestAsync(Guid depositId, CancellationToken cancellationToken = default);
}