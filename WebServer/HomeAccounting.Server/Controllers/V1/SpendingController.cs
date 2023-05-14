using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/spendings")]
public class SpendingController : BaseController
{
    private readonly ISpendingService _spendingService;

    public SpendingController(
        IServiceProvider services,
        ISpendingService spendingService
    ) : base(services) => _spendingService = spendingService;

    [HttpGet]
    public async Task<IActionResult> GetSpendingAsync(
        [FromQuery] Guid userId,
        [FromQuery] Guid spendingId,
        CancellationToken cancellationToken
    ) => Ok(
        await _spendingService
            .GetSpendingAsync(
                userId,
                spendingId,
                cancellationToken
            )
    );

    [HttpGet("average")]
    public async Task<IActionResult> GetAverageSpendingAsync(
        CancellationToken cancellationToken
    ) => Ok(
        await _spendingService
            .GetAverageSpendingAsync(
                CurrentUserId,
                cancellationToken
            )
    );

    [HttpPost]
    public async Task<IActionResult> CreateSpendingAsync(
        [FromBody] CreateSpendingModel createSpendingModel,
        CancellationToken cancellationToken
    )
    {
        await ValidateAsync(createSpendingModel, cancellationToken);

        await _spendingService.CreateSpendingAsync(createSpendingModel, cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateSpendingAsync(
        [FromBody] UpdateSpendingModel updateSpendingModel,
        CancellationToken cancellationToken
    )
    {
        await ValidateAsync(updateSpendingModel, cancellationToken);

        await _spendingService.UpdateSpendingAsync(
            updateSpendingModel, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteSpendingAsync(
        [FromQuery] Guid spendingId,
        CancellationToken cancellationToken
    )
    {
        await _spendingService.DeleteSpendingAsync(spendingId, cancellationToken);

        return Ok();
    }
}