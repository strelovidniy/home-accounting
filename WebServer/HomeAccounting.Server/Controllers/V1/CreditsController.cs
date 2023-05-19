using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models.Create;
using HomeAccounting.Models.Update;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/credits")]
public class CreditsController : BaseController
{
    private readonly ICreditService _creditService;

    public CreditsController(
        IServiceProvider services,
        ICreditService creditService
    ) : base(services) => _creditService = creditService;

    [HttpPost]
    public async Task<IActionResult> CreateCreditAsync(
        [FromBody] CreateCreditModel createCreditModel,
        CancellationToken cancellationToken
    )
    {
        await _creditService.CreateCreditAsync(createCreditModel, cancellationToken);

        return Ok();
    }

    [HttpPut]
    public async Task<IActionResult> UpdateCreditAsync(
        [FromBody] UpdateCreditModel updateCreditModel,
        CancellationToken cancellationToken
    )
    {
        await _creditService.UpdateCreditAsync(
            updateCreditModel, cancellationToken);

        return Ok();
    }

    [HttpDelete]
    public async Task<IActionResult> DeleteCreditAsync(
        [FromQuery] Guid creditId,
        CancellationToken cancellationToken
    )
    {
        await _creditService.DeleteCreditAsync(creditId, cancellationToken);

        return Ok();
    }
}