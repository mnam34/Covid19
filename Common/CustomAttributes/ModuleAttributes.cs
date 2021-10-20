using System;
namespace Common.CustomAttributes
{
    /// <summary>
    /// Enum để đánh dấu chức năng thuộc nhóm chức năng nào
    /// </summary>
    [AttributeUsage(AttributeTargets.All)]
    public class ModuleGroupAttribute : Attribute
    {
        public string ModuleGroupName { get; set; }
        public int ModuleGroupCode { get; set; }
        public ModuleGroupAttribute()
        {
        }
    }
}
