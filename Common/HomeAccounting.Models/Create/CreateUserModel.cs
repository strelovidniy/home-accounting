namespace HomeAccounting.Models.Create;

public class CreateUserModel : IValidatableModel
{
    public Guid InvitationToken { get; set; }

    public string Password { get; set; }

    public string ConfirmPassword { get; set; }

    public string FirstName { get; set; }

    public string LastName { get; set; }
}