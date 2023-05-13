using System.Security.Claims;
using FluentValidation;
using HomeAccounting.Domain.Exceptions;
using HomeAccounting.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.Base;

[Authorize]
[ApiController]
public class BaseController : ControllerBase
{
    private readonly IServiceProvider _services;

    protected Guid CurrentUserId => Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)
        ?? throw new ApiException(Data.Enums.StatusCode.Unauthorized));

    public BaseController(
        IServiceProvider services
    ) => _services = services;

    protected Task ValidateAsync<T>(T validatableModel, CancellationToken cancellationToken = default)
        where T : class, IValidatableModel =>
        _services.GetRequiredService<IValidator<T>>().ValidateAndThrowAsync(validatableModel, cancellationToken);
}