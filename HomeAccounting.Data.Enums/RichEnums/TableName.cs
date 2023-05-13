using RichEnum;

namespace HomeAccounting.Data.Enums.RichEnums;

public class TableName : RichEnum<string>
{
    public static TableName Users => new("Users");

    private TableName(string value) : base(value)
    {
    }
}