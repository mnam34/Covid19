using Common.CustomAttributes;
using System;
using System.ComponentModel;
namespace Entities.Enums
{
    /// <summary>
    /// Danh sách mã chức năng của hệ thống
    /// </summary>
    public enum ModuleEnum
    {
        [ModuleGroup(ModuleGroupCode = 5, ModuleGroupName = "Quản trị hệ thống")]
        [Description("Không kiểm tra quyền, chỉ kiểm tra ứng dụng")]
        [Action(ActionType.NoCheck)]
        NoCheck = -1,

        [ModuleGroup(ModuleGroupCode = 5, ModuleGroupName = "Quản trị hệ thống")]
        [Description("Quản lý tài khoản")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        Account = 500,
        [ModuleGroup(ModuleGroupCode = 5, ModuleGroupName = "Quản trị hệ thống")]
        [Description("Quản lý vai trò người dùng (Nhóm người dùng, nhóm quyền)")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        Role = 501,
        [ModuleGroup(ModuleGroupCode = 5, ModuleGroupName = "Quản trị hệ thống")]
        [Description("Xem danh sách người dùng theo nhóm quyền")]
        [Action(ActionType.Read, ActionType.Delete)]
        AssignedRole = 502,
        [ModuleGroup(ModuleGroupCode = 5, ModuleGroupName = "Quản trị hệ thống")]
        [Description("Xem danh sách người dùng được phân đơn vị theo dõi công việc")]
        [Action(ActionType.Read, ActionType.Delete)]
        AssignedDivisionWT = 503,

        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Hình thức cách ly")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        QuarantineType = 600,
        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Nơi phát hiện ca bệnh")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        DetectedPlace = 601,
        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Cơ sở điều trị bệnh (tại nhà, bệnh viện dã chiến, bệnh viện...)")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        TreatmentFacility = 602,
        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Khu cách ly tập trung")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        QuarantinePlace = 603,
        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Khu vực/vùng/điểm bùng phát dịch bệnh")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        EpidemicArea = 604,
        [ModuleGroup(ModuleGroupCode = 6, ModuleGroupName = "Quản trị danh mục")]
        [Description("Danh mục Phân loại Mức độ nguy cơ lây nhiễm")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        RiskClassification = 605,

        [ModuleGroup(ModuleGroupCode = 1, ModuleGroupName = "Quản lý ca Covid F-case")]
        [Description("Quản lý ca bệnh F0")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        FCase = 100,
        [ModuleGroup(ModuleGroupCode = 1, ModuleGroupName = "Quản lý ca Covid F-case")]
        [Description("Quản lý kết quả xét ngiệm")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        FCaseTestResult = 101,

        [ModuleGroup(ModuleGroupCode = 1, ModuleGroupName = "Quản lý ca Covid F-case")]
        [Description("Quản lý danh sách Fx")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        FCaseFx = 102,

        [ModuleGroup(ModuleGroupCode = 1, ModuleGroupName = "Quản lý ca Covid F-case")]
        [Description("Quản lý giấy tờ, tài liệu của Fx")]
        [Action(ActionType.Read, ActionType.Create, ActionType.Update, ActionType.Delete)]
        FCaseDocument = 103,

        [ModuleGroup(ModuleGroupCode = 1, ModuleGroupName = "Quản lý ca Covid F-case")]
        [Description("Xem báo cáo F-case")]
        [Action(ActionType.Read)]
        FCaseReport = 104,
    }
}
