﻿using System.Reflection;
using HomeAccounting.Data.Enums.RichEnums;
using HomeAccounting.Server.Constants;
using HomeAccounting.Server.Controllers.Base;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HomeAccounting.Server.Controllers.V1;

[Route("api/v1/static-files")]
[AllowAnonymous]
[ResponseCache(CacheProfileName = ResponseCacheProfile.StaticDataCacheProfile)]
public class StaticFilesController : BaseController
{
    public StaticFilesController(IServiceProvider services) : base(services)
    {
    }

    [HttpGet("icon")]
    public IActionResult GetKuliaBIcon() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.Icon)!,
        ContentType.ImagePng
    );

    [HttpGet("twitter-icon")]
    public IActionResult GetTwitterIcon() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.TwitterIcon)!,
        ContentType.ImagePng
    );

    [HttpGet("facebook-icon")]
    public IActionResult GetFacebookIcon() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.FacebookIcon)!,
        ContentType.ImagePng
    );

    [HttpGet("instagram-icon")]
    public IActionResult GetInstagramIcon() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.InstagramIcon)!,
        ContentType.ImagePng
    );

    [HttpGet("white-background")]
    public IActionResult GetWhiteBackground() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.WhiteBackground)!,
        ContentType.ImagePng
    );

    [HttpGet("divider")]
    public IActionResult GetDivider() => File(
        Assembly
            .GetExecutingAssembly()
            .GetManifestResourceStream(ImageLocation.Divider)!,
        ContentType.ImagePng
    );
}