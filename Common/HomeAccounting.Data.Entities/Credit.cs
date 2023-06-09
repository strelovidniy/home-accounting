﻿using EntityFrameworkCore.RepositoryInfrastructure;

namespace HomeAccounting.Data.Entities;

public class Credit : Entity, IEntity
{
    public Guid UserId { get; set; }

    public decimal Amount { get; set; }

    public string? Description { get; set; }

    public User? User { get; set; }
}