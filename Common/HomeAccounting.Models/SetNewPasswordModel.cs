namespace HomeAccounting.Models;

public class SetNewPasswordModel : IValidatableModel
{
    public Guid VerificationCode { get; set; }

    public string NewPassword { get; set; }

    public string ConfirmNewPassword { get; set; }
}