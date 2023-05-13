﻿using RichEnum;

namespace HomeAccounting.Data.Enums.RichEnums;

public class TableSchema : RichEnum<string>
{
    public static TableSchema Dbo = new("dbo");

    private TableSchema(string value) : base(value)
    {
    }
}