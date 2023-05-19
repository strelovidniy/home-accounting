using RichEnum;

namespace HomeAccounting.Data.Enums.RichEnums;

public class TableName : RichEnum<string>
{
    public static TableName Users => new("Users");

    public static TableName Incomings => new("Incomings");

    public static TableName Spendings => new("Spendings");
    public static TableName Credits => new("Credits");

    private TableName(string value) : base(value)
    {
    }
}