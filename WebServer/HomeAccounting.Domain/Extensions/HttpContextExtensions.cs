using System.Security.Claims;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Exceptions;
using Microsoft.AspNetCore.Http;

namespace HomeAccounting.Domain.Extensions;

public static class HttpContextExtensions
{
    public static Guid GetCurrentUserId(this IHttpContextAccessor httpContextAccessor) =>
        httpContextAccessor.HttpContext.GetCurrentUserId();

    public static Guid GetCurrentUserId(this HttpContext? httpContext) =>
        Guid.Parse(httpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier)
            ?? throw new ApiException(StatusCode.Unauthorized));
}