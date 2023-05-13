using HomeAccounting.Data.Enums;

namespace HomeAccounting.Domain.Exceptions;

public class ApiException : Exception
{
    public StatusCode StatusCode { get; }

    public ApiException(StatusCode statusCode) => StatusCode = statusCode;
}