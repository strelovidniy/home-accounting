namespace HomeAccounting.Models.Change;

public class ChangePasswordModel : IValidatableModel
{
    public string OldPassword { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }
}