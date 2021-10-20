using System.ComponentModel;
namespace Entities.Enums
{
    /// <summary>
    /// Danh sách các nhóm chức năng
    /// </summary>
    public enum ModuleGroupEnum
    {
        [Description("Quản lý ca Covid-19 F-case")]
        Covid = 1,
        [Description("Quản trị hệ thống")]
        QuanTriHeThong = 5,
        [Description("Quản trị danh mục")]
        QuanTriDanhMuc = 6,
    }
}
