﻿using AutoMapper;
using HomeAccounting.Data.Enums;
using HomeAccounting.Domain.Helpers;
using HomeAccounting.Models.Create;

namespace HomeAccounting.Domain.Mapper.Converters.User;

internal class CreateUserModelToUserConverter : ITypeConverter<CreateUserModel, Data.Entities.User>
{
    public Data.Entities.User Convert(
        CreateUserModel createUserModel,
        Data.Entities.User user,
        ResolutionContext context
    ) => new()
    {
        Email = createUserModel.Email,
        FirstName = createUserModel.FirstName,
        LastName = createUserModel.LastName,
        PasswordHash = PasswordHasher.GetHash(createUserModel.Password),
        InvitationToken = null,
        Status = UserStatus.Active
    };
}