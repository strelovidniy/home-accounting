﻿namespace HomeAccounting.Models.Views;

public class DepositView
{
    public Guid Id { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public DateTime DepositDate { get; set; }

    public DateTime? DepositUpdatedAt { get; set; }

    public UserView? User { get; set; }
}