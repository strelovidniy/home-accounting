using FluentValidation;
using HomeAccounting.Data.Enums;
using HomeAccounting.Data.Enums.RichEnums;
using HomeAccounting.Domain.Exceptions;
using HomeAccounting.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace HomeAccounting.Domain.Middleware;

public class ApiExceptionFilter : IExceptionFilter
{
    private readonly ILogger<ApiExceptionFilter> _logger;

    public ApiExceptionFilter(ILogger<ApiExceptionFilter> logger) => _logger = logger;

    public void OnException(ExceptionContext context)
    {
        StatusCode statusCode = default;

        switch (context.Exception)
        {
            case ApiException apiException:
                _logger.LogError(apiException, ErrorMessage.ApiExceptionOccurred);

                statusCode = apiException.StatusCode;

                break;

            case ValidationException validationException:
                _logger.LogWarning(validationException, ErrorMessage.ValidationExceptionOccurred);

                int.TryParse(validationException.Errors.FirstOrDefault()?.ErrorCode, out var errorCode);

                statusCode = (StatusCode) errorCode;

                break;

            default:
                _logger.LogCritical(context.Exception, ErrorMessage.InternalServerErrorOccurred);

                break;
        }

        context.Result = new ObjectResult(new ApiErrorResult(statusCode));

        context.HttpContext.Response.StatusCode = statusCode is StatusCode.MethodNotAvailable ? 500 : 400;
        context.HttpContext.Response.ContentType = ContentType.ApplicationProblem;
        context.ExceptionHandled = true;
    }
}