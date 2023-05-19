﻿using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/Credits")]
public class CreditsController : BaseController
{
    private readonly ICreditService _CreditService;

    public CreditsController(
        IServiceProvider services,
        ICreditService CreditService
    ) : base(services) => _CreditService = CreditService;
    
    [HttpPost]
    public async Task<IActionResult> CreateCreditAsync(
        [FromBody] CreateCreditModel createCreditModel,
        CancellationToken cancellationToken
    )
    {
        await _CreditService.CreateCreditAsync(createCreditModel, cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCreditAsync(
        [FromBody] UpdateCreditModel updateCreditModel,
        CancellationToken cancellationToken
    )
    {
        await _CreditService.UpdateCreditAsync(
            updateCreditModel, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCreditAsync(
        [FromQuery] Guid CreditId,
        CancellationToken cancellationToken
    )
    {
        await _CreditService.DeleteCreditAsync(CreditId, cancellationToken);

        return Ok();
    }
}