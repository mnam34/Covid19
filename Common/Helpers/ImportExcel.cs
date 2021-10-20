using System;
[AttributeUsage(AttributeTargets.All)]
public class ImportExcel : Attribute
{
    public int ColumnIndex { get; set; }
    public string Format { get; set; }

    public string ColumDataType { get; set; }
    public int IdentityGroup { get; set; }
}