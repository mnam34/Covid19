using System;
using System.Linq;
using System.Reflection;
namespace Common.CustomAttributes
{
    public static class AttributeReader
    {
        //Cách dùng: var myDescription = myEnum.Description();
        public static string Description(this Enum value)
        {
            // get attributes  
            var field = value.GetType().GetField(value.ToString());
            var attributes = field.GetCustomAttributes(false);

            // Description is in a hidden Attribute class called DisplayAttribute
            // Not to be confused with DisplayNameAttribute
            dynamic displayAttribute = null;

            if (attributes.Any())
            {
                displayAttribute = attributes.ElementAt(0);
            }

            // return description
            return displayAttribute?.Description ?? "Description Not Found";
        }
        public static Att GetPropertyAttribute<Att>(Type t, string propName) where Att : Attribute
        {
            var MyMemberInfo = t.GetProperty(propName);
            if (MyMemberInfo == null) return null;
            Att result = (Att)Attribute.GetCustomAttribute(MyMemberInfo, typeof(Att));
            return result;
        }
        ////Lấy tất cả attributes của Type t
        //public static Att[] GetTypeAttributes<Att>(Type t) where Att : Attribute
        //{
        //    Att[] result = (Att[])Attribute.GetCustomAttributes(t, typeof(Att));
        //    return result;
        //}

        ////Lấy 1 attribute của Type t
        //public static Att GetTypeAttribute<Att>(Type t) where Att : Attribute
        //{
        //    Att result = (Att)Attribute.GetCustomAttribute(t, typeof(Att));
        //    return result;
        //}
        ////Lấy attribute của 1 method
        //public static Att GetMethodAttribute<Att>(Type t, string methodName) where Att : Attribute
        //{
        //    var MyMemberInfo = t.GetMethods().Where(o => o.Name == methodName).ToList().FirstOrDefault();
        //    if (MyMemberInfo == null) return null;
        //    Att result = (Att)Attribute.GetCustomAttribute(MyMemberInfo, typeof(Att));
        //    return result;
        //}



        //public static Att GetPropertyAttribute<Att>(Type t, string propName) where Att : Attribute
        //{
        //    var MyMemberInfo = t.GetProperty(propName);
        //    if (MyMemberInfo == null) return null;
        //    Att result = (Att)Attribute.GetCustomAttribute(MyMemberInfo, typeof(Att));
        //    return result;
        //}
    }
}
