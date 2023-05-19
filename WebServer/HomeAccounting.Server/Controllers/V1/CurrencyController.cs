using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/currency")]
public class CurrencyController : BaseController
{
    private readonly ICurrencyService _currencyService;

    public CurrencyController(
        IServiceProvider services,
        ICurrencyService currencyService
    ) : base(services) => _currencyService = currencyService;

    [HttpGet]
    public async Task<IActionResult> GetCurrenciesAsync(
        CancellationToken cancellationToken = default
    ) => Ok(
        await _currencyService.GetCurrenciesAsync(
            cancellationToken
        )
    );
}