using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Settings.Abstraction;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Domain.Helpers;

public static class JwtHelper
{
    public static TokenValidationParameters GetTokenValidationParameters(IJwtSettings jwtSettings) =>
        new()
        {
            ValidateIssuer = true,
            ValidIssuers = new[] { jwtSettings.Issuer },

            ValidateAudience = false,

            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(jwtSettings.TokenSecretString.ToByteArray()),
            ValidateLifetime = true,
            ClockSkew = TimeSpan.Zero
        };
}