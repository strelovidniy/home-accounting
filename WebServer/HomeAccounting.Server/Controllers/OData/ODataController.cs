using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Models.Views;
using HomeAccounting.UI.Domain.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Extensions;
using Microsoft.AspNetCore.OData.Query;
using ODataControllerBase = Microsoft.AspNetCore.OData.Routing.Controllers.ODataController;

namespace HomeAccounting.Server.Controllers.OData;

[Authorize]
[Route("api/odata")]
public class ODataController : ODataControllerBase
{
    private readonly IMapper _mapper;

    private Guid CurrentUserId => HttpContext.GetCurrentUserId();

    public ODataController(
        IMapper mapper
    ) => _mapper = mapper;

    [HttpGet("users")]
    public IActionResult GetUsers(
        ODataQueryOptions<User> options,
        [FromServices] IRepository<User> repository
    ) => Ok(new ODataResponse<UserView>(
        options.Context.ToString() ?? string.Empty,
        _mapper
            .Map<List<UserView>>(
                options
                    .ApplyTo(
                        repository
                            .Query()
                    )
                    .Cast<User>()
                    .ToList()
            ),
        (int) (options.Request.ODataFeature().TotalCount ?? 0)
    ));

    [HttpGet("spendings")]
    public IActionResult GetSpendings(
        ODataQueryOptions<Spending> options,
        [FromServices] IRepository<Spending> repository
    ) => Ok(new ODataResponse<SpendingView>(
        options.Context.ToString() ?? string.Empty,
        _mapper
            .Map<List<SpendingView>>(
                options
                    .ApplyTo(
                        repository
                            .Query()
                            .Where(spending => spending.UserId == CurrentUserId)
                    )
                    .Cast<Spending>()
                    .ToList()
            ),
        (int) (options.Request.ODataFeature().TotalCount ?? 0)
    ));

    [HttpGet("incomings")]
    public IActionResult GetIncomings(
        ODataQueryOptions<Incoming> options,
        [FromServices] IRepository<Incoming> repository
    ) => Ok(new ODataResponse<IncomingView>(
        options.Context.ToString() ?? string.Empty,
        _mapper.Map<List<IncomingView>>(
            options
                .ApplyTo(
                    repository
                        .Query()
                        .Where(incoming => incoming.UserId == CurrentUserId)
                )
                .Cast<Incoming>()
                .ToList()
        ),
        (int) (options.Request.ODataFeature().TotalCount ?? 0)
    ));
    
    [HttpGet("credits")]
    public IActionResult GetCredits(
        ODataQueryOptions<Credit> options,
        [FromServices] IRepository<Credit> repository
    ) 
    {
        return Ok(new ODataResponse<CreditView>(
            options.Context.ToString() ?? string.Empty,
            _mapper.Map<List<CreditView>>(
                options
                    .ApplyTo(
                        repository
                            .Query()
                            .Where(credit => credit.UserId == CurrentUserId)
                    )
                    .Cast<Credit>()
                    .ToList()
            ),
            (int)(options.Request.ODataFeature().TotalCount ?? 0)
        ));
    }
    
    [HttpGet("deposits")]
    public IActionResult GetCredits(
        ODataQueryOptions<Deposit> options,
        [FromServices] IRepository<Deposit> repository
    ) 
    {
        return Ok(new ODataResponse<DepositView>(
            options.Context.ToString() ?? string.Empty,
            _mapper.Map<List<DepositView>>(
                options
                    .ApplyTo(
                        repository
                            .Query()
                            .Where(credit => credit.UserId == CurrentUserId)
                    )
                    .Cast<Deposit>()
                    .ToList()
            ),
            (int)(options.Request.ODataFeature().TotalCount ?? 0)
        ));
    }
}