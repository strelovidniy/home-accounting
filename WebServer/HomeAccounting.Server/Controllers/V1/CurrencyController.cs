using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/currency")]
public class CurrencyController : BaseController
{
    private readonly ICurrencyService _currencyService;
    private readonly IMonoApiService _monoApiService;

    public CurrencyController(
        IServiceProvider services,
        ICurrencyService currencyService, IMonoApiService monoApiService) : base(services)
    {
        _currencyService = currencyService;
        _monoApiService = monoApiService;
    }

    [HttpGet]
    public async Task<IActionResult> GetCurrenciesAsync(
        CancellationToken cancellationToken = default
    ) => Ok(
        await _currencyService.GetCurrenciesAsync(
            cancellationToken
        )
    );
    
    [HttpGet]
    [Route("statements")]
    public async Task<IActionResult> GetStatementAsync(
        CancellationToken cancellationToken = default
    ) => Ok(
        await _monoApiService.ReturnStatementAsync(CurrentUserId, DateTime.MinValue, DateTime.MaxValue, cancellationToken));
}