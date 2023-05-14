using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/incomings")]
public class IncomingController : BaseController
{
    private readonly IIncomingService _incomingService;

    public IncomingController(
        IServiceProvider services,
        IIncomingService incomingService
    ) : base(services) => _incomingService = incomingService;

    [HttpGet]
    public async Task<IActionResult> GetIncomingAsync(
        [FromQuery] Guid userId,
        [FromQuery] Guid incomingId,
        CancellationToken cancellationToken
    ) => Ok(
        await _incomingService
            .GetIncomingAsync(
                userId,
                incomingId,
                cancellationToken
            )
    );

    [HttpPost]
    public async Task<IActionResult> CreateIncomingAsync(
        [FromBody] CreateIncomingModel createIncomingModel,
        CancellationToken cancellationToken
    )
    {
        await ValidateAsync(createIncomingModel, cancellationToken);

        await _incomingService.CreateIncomingAsync(createIncomingModel, cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateIncomingAsync(
        [FromBody] UpdateIncomingModel updateIncomingModel,
        CancellationToken cancellationToken
    )
    {
        await ValidateAsync(updateIncomingModel, cancellationToken);

        await _incomingService.UpdateIncomingAsync(
            updateIncomingModel, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteIncomingAsync(
        [FromQuery] Guid incomingId,
        CancellationToken cancellationToken
    )
    {
        await _incomingService.DeleteIncomingAsync(incomingId, cancellationToken);

        return Ok();
    }
}