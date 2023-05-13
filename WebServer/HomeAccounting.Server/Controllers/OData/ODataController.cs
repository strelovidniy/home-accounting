using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using ODataControllerBase = Microsoft.AspNetCore.OData.Routing.Controllers.ODataController;

namespace HomeAccounting.Server.Controllers.OData;

[Route("api/odata")]
public class ODataController : ODataControllerBase
{
    [HttpGet("users")]
    [EnableQuery]
    public IQueryable<User> GetUsers(
        [FromServices] IRepository<User> repository
    ) => repository.Query();
}