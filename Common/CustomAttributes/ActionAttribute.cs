using System;
namespace Common.CustomAttributes
{
    /// <summary>
    /// Enum để đánh dấu 1 chức năng có những hành động gì: Thêm, sửa, xóa, duyệt....
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ActionAttribute : Attribute
    {
        public int[] Actions { get; set; }
        public ActionAttribute(params int[] actions)
        {
            Actions = actions;
        }
    }
}
