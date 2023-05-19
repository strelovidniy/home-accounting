namespace HomeAccounting.Data.Enums;

public enum StatusCode
{
    MethodNotAvailable = 0,
    Unauthorized = 1,
    Forbidden = 2,

    SpendingNotFound = 101,
    AccountNotFound = 102,
    UserNotFound = 103,
    IncomingNotFound = 104,

    DescriptionTooLong = 201,

    EmailSendingError = 301,


    YouAreNotAllowedToSeeThisSpending = 701,
    YouAreNotAllowedToSeeThisIncoming = 702,
    YouCanOnlyAddIncomingForYourself = 703,
    YouCanOnlyAddSpendingForYourself = 704,
    YouCanOnlyUpdateIncomingForYourself = 705,
    YouCanOnlyUpdateSpendingForYourself = 706,
    YouCanOnlyDeleteIncomingForYourself = 707,
    YouCanOnlyDeleteSpendingForYourself = 708,


    LastNameRequired = 1021,
    FirstNameRequired = 1022,
    PasswordRequired = 1024,
    EmailRequired = 1025,
    VerificationCodeRequired = 1027,
    ConfirmPasswordRequired = 1028,

    PasswordLengthExceeded = 1109,

    PasswordMustHaveAtLeast8Characters = 1201,
    PasswordMustHaveNotMoreThan32Characters = 1202,
    PasswordMustHaveAtLeastOneUppercaseLetter = 1203,
    PasswordMustHaveAtLeastOneLowercaseLetter = 1204,
    PasswordMustHaveAtLeastOneDigit = 1205,
    AmountMustBeGreaterThanZero = 1206,

    InvalidEmailFormat = 1301,
    InvalidEmailModel = 1302,
    InvalidVerificationCode = 1303,
    InvalidFileExtension = 1304,

    FirstNameShouldNotContainWhiteSpace = 1401,
    LastNameShouldNotContainWhiteSpace = 1402,

    IncorrectPassword = 1501,
    PasswordsDoNotMatch = 1502,
    OldPasswordIncorrect = 1503,

    EmailAlreadyExists = 1603
}