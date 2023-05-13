using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using EntityFrameworkCore.RepositoryInfrastructure;
using HomeAccounting.Data.Entities;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Extensions;
using HomeAccounting.Domain.Helpers;
using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Domain.Settings.Abstraction;
using HomeAccounting.Domain.Validators.Runtime;
using HomeAccounting.Models;
using HomeAccounting.Models.Views;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace HomeAccounting.Domain.Services.Realization;

internal class AuthService : IAuthService
{
    private readonly IRepository<User> _userRepository;
    private readonly IHttpContextAccessor _httpContextAccessor;
    private readonly IUserService _userService;
    private readonly IJwtSettings _jwtSettings;

    public AuthService(
        IRepository<User> userRepository,
        IHttpContextAccessor httpContextAccessor,
        IUserService userService,
        IJwtSettings jwtSettings
    )
    {
        _userRepository = userRepository;
        _httpContextAccessor = httpContextAccessor;
        _userService = userService;
        _jwtSettings = jwtSettings;
    }

    public Task<User?> GetCurrentUserAsync(
        CancellationToken cancellationToken = default
    ) => _userService.GetUserAsync(
        _httpContextAccessor.GetCurrentUserId(),
        cancellationToken
    );

    public async Task<LoginView?> LoginAsync(
        LoginModel model,
        CancellationToken cancellationToken = default
    )
    {
        var user = await _userRepository
            .Query()
            .FirstOrDefaultAsync(
                u => u.Email == model.Email,
                cancellationToken
            );

        RuntimeValidator.Assert(user is not null, StatusCode.AccountNotFound);

        RuntimeValidator.Assert(
            user!.PasswordHash == PasswordHasher.GetHash(model.Password),
            StatusCode.IncorrectPassword
        );

        var refreshToken = Guid.NewGuid();
        var refreshTokenExpireAt = DateTime.UtcNow.AddSeconds(_jwtSettings.SecondsToExpireRefreshToken);

        user!.RefreshToken = refreshToken;
        user.RefreshTokenExpirationDate = refreshTokenExpireAt;

        await _userRepository.SaveChangesAsync(cancellationToken);

        var authTokenModel = GenerateToken(user);

        authTokenModel.RefreshToken = refreshToken;

        return new LoginView
        {
            User = user,
            Token = authTokenModel
        };
    }

    private AuthToken GenerateToken(User user)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        var claims = new List<Claim>
        {
            new(ClaimTypes.NameIdentifier, user.Id.ToString())
            //new(ClaimTypes.Role, user.Role?.Name ?? string.Empty)
        };

        var tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = new ClaimsIdentity(claims),
            Expires = DateTime.UtcNow.AddSeconds(_jwtSettings.SecondsToExpireToken),
            Issuer = _jwtSettings.Issuer,
            IssuedAt = DateTime.UtcNow,
            NotBefore = DateTime.UtcNow,
            SigningCredentials = new SigningCredentials(
                new SymmetricSecurityKey(_jwtSettings.TokenSecretString.ToByteArray()),
                SecurityAlgorithms.HmacSha512Signature
            )
        };

        var token = tokenHandler.CreateToken(tokenDescriptor);

        return new AuthToken
        {
            Token = tokenHandler.WriteToken(token),
            ExpireAt = token.ValidTo.ToUniversalTime().ToString("yyyy-MM-ddTHH:mm:ssZ")
        };
    }
}