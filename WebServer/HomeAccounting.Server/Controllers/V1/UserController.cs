using HomeAccounting.Domain.Services.Abstraction;
using HomeAccounting.Models;
using HomeAccounting.Models.Change;
using HomeAccounting.Models.Create;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/users")]
public class UserController : BaseController
{
    private readonly IUserService _userService;

    public UserController(
        IServiceProvider services,
        IUserService userService
    ) : base(services) => _userService = userService;


    [AllowAnonymous]
    [HttpPost("reset-password")]
    public async Task<IActionResult> ResetPasswordAsync(
        [FromBody] ResetPasswordModel resetPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(resetPasswordModel, cancellationToken);

        await _userService.ResetPasswordAsync(resetPasswordModel, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("set-new-password")]
    public async Task<IActionResult> SetNewPasswordAsync(
        [FromBody] SetNewPasswordModel setNewPasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(setNewPasswordModel, cancellationToken);

        await _userService.SetNewPasswordAsync(setNewPasswordModel, cancellationToken);

        return Ok();
    }

    [HttpPut("change-password")]
    public async Task<IActionResult> ChangePasswordAsync(
        [FromBody] ChangePasswordModel changePasswordModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(changePasswordModel, cancellationToken);

        await _userService.ChangePasswordAsync(changePasswordModel, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpPost("sign-up")]
    public async Task<IActionResult> SignUpAsync(
        [FromBody] CreateUserModel createUserModel,
        CancellationToken cancellationToken = default
    )
    {
        await ValidateAsync(createUserModel, cancellationToken);

        await _userService.CreateUserAsync(createUserModel, cancellationToken);

        return Ok();
    }

    [HttpPost("change-avatar")]
    public async Task<IActionResult> ChangeAvatarAsync(
        [FromForm] string fileContent,
        CancellationToken cancellationToken = default
    )
    {
        var decodedContent = Convert.FromBase64String(fileContent);

        await _userService.ChangeAvatarAsync(decodedContent, cancellationToken);

        return Ok();
    }

    [AllowAnonymous]
    [HttpGet("avatars/{imageName}")]
    public async Task<IActionResult> GetAvatarAsync(
        string imageName,
        CancellationToken cancellationToken = default
    )
    {
        var avatar = await System.IO.File.ReadAllBytesAsync(
            Path.Combine(
                Directory.GetCurrentDirectory(),
                "Files",
                "Avatars",
                imageName
            ),
            cancellationToken
        );

        return File(avatar, "image/png");
    }

    [HttpPut("monobank-token")]
    public async Task<IActionResult> SaveMonobankToken(
        [FromBody] SetMonobankTokenModel monobankToken,
        CancellationToken cancellationToken = default
    )
    {
        await _userService.SaveMonobankApiTokenAsync(monobankToken, cancellationToken);

        return Ok();
    }
}