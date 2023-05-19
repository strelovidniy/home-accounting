using HomeAccounting.Data.Enums;

namespace HomeAccounting.Domain.Models;

public record CurrencyModel(
    CurrencyCode CurrencyCodeA,
    CurrencyCode CurrencyCodeB,
    decimal RateBuy,
    decimal RateSell
);