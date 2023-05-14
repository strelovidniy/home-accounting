﻿using HomeAccounting.Data.Enums;

namespace HomeAccounting.Models.Views;

public class UserView
{
    public Guid Id { get; set; }

    public string Email { get; set; } = null!;

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string FullName => $"{FirstName} {LastName}";

    public UserStatus Status { get; set; }

    public string? ImageDataUrl { get; set; }
}