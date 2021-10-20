using Common.CustomAttributes;
using System.Collections.Generic;

using System.Reflection;
namespace Common.Helpers
{
    public class ClassReader<C> where C : class
    {
        public static string GetClassName()
        {
            return (typeof(C)).Name;
        }
        public static List<Property> ReadEntity(C obj)
        {
            var t = typeof(C);
            var result = new List<Property>();
            foreach (var prop in t.GetProperties())
            {
                var p = new Property
                {
                    //Lay datatype
                    DataType = prop.PropertyType.ToString().ToLower(),
                    //LayName
                    Name = prop.Name
                };
                //Kiem tra truong duoc danh dau key
                //var key = AttributeReader.GetPropertyAttribute<Key>(t, prop.Name);
                //p.IsKey = key != null;
                //Kiem tra truong autonumber
                var autoNumber = AttributeReader.GetPropertyAttribute<AutoNumberAttribute>(t, prop.Name);
                p.IsAutoNumber = autoNumber != null;
                //Lay value
                p.Value = prop.GetValue(obj, null);
                //Lay className
                p.ClassName = GetClassName();
                result.Add(p);
            }
            return result;
        }

        public PropertyInfo GetProperty(string propName)
        {
            var t = typeof(C);
            return t.GetProperty(propName);
        }
        public object GetPropertyValue(C obj, string propName)
        {
            try
            {
                var prop = GetProperty(propName);
                var propertyValue = prop.GetValue(obj, null);
                return propertyValue;
            }
            catch
            {
                return null;
            }
        }
        public bool SetPropValue(C obj, string propName, object value)
        {
            try
            {
                var prop = GetProperty(propName);
                if (prop != null)
                {
                    prop.SetValue(obj, value, null);
                }
                else return false;
            }
            catch
            {
                return false;
            }
            return true;
        }
        public static List<Property> ReadEntity()
        {
            var t = typeof(C);
            var result = new List<Property>();
            foreach (var prop in t.GetProperties())
            {
                var p = new Property();
                //Lay datatype
                p.DataType = prop.PropertyType.ToString().ToLower();
                //LayName
                p.Name = prop.Name;
                //prop.SetValue(
                //Kiem tra truong duoc danh dau key
                //var key = AttributeReader.GetPropertyAttribute<KeyAttribute>(t, prop.Name);
                //p.IsKey = key != null;
                //Kiem tra truong autonumber
                var autoNumber = AttributeReader.GetPropertyAttribute<AutoNumberAttribute>(t, prop.Name);
                p.IsAutoNumber = autoNumber != null;
                //Lay className
                p.ClassName = GetClassName();
                result.Add(p);
            }
            return result;
        }
    }
}
