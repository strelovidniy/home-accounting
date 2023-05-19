using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/deposits")]
public class DepositsController : BaseController
{
    private readonly IDepositService _depositService;

    public DepositsController(
        IServiceProvider services,
        IDepositService DepositService
    ) : base(services) => _depositService = DepositService;
    
    [HttpPost]
    public async Task<IActionResult> CreateDepositAsync(
        [FromBody] CreateDepositModel createDepositModel,
        CancellationToken cancellationToken
    )
    {
        await _depositService.CreateDepositAsync(createDepositModel, cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateDepositAsync(
        [FromBody] UpdateDepositModel updateDepositModel,
        CancellationToken cancellationToken
    )
    {
        await _depositService.UpdateDepositAsync(
            updateDepositModel, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteDepositAsync(
        [FromQuery] Guid DepositId,
        CancellationToken cancellationToken
    )
    {
        await _depositService.DeleteDepositAsync(DepositId, cancellationToken);

        return Ok();
    }
}