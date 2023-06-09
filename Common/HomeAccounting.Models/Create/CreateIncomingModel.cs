﻿namespace HomeAccounting.Models.Create;

public class CreateIncomingModel : IValidatableModel
{
    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public Guid UserId { get; set; }
}