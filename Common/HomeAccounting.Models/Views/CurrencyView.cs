namespace HomeAccounting.Models.Views;

public record CurrencyView(
    string CurrencyA,
    string CurrencyB,
    decimal RateBuy,
    decimal RateSell
);