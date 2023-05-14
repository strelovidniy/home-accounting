using AutoMapper;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Models.Views;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
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
    ) => Ok(
        GetMappedList<User, UserView>(
            options,
            repository.Query()
        )
    );

    [HttpGet("spendings")]
    public IActionResult GetSpendings(
        ODataQueryOptions<Spending> options,
        [FromServices] IRepository<Spending> repository
    ) => Ok(
        GetMappedList<Spending, SpendingView>(
            options,
            repository
                .Query()
                .Where(spending => spending.UserId == CurrentUserId)
        )
    );

    [HttpGet("incomings")]
    public IActionResult GetIncomings(
        ODataQueryOptions<Incoming> options,
        [FromServices] IRepository<Incoming> repository
    ) => Ok(
        GetMappedList<Incoming, IncomingView>(
            options,
            repository
                .Query()
                .Where(incoming => incoming.UserId == CurrentUserId)
        )
    );

    private IEnumerable<TResult> GetMappedList<TEntity, TResult>(
        ODataQueryOptions<TEntity> options,
        IQueryable<TEntity> repository
    ) where TEntity : class, IEntity where TResult : class =>
        _mapper.Map<IEnumerable<TResult>>(options.ApplyTo(repository).Cast<TEntity>().ToList());
}