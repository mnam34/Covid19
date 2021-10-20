using Common.CustomAttributes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Helpers
{
    public class ImportHelper<T> where T : class, new()
    {
        public T GetObject(DataRow row)
        {
            var result = new T();
            ClassReader<T> objmng = new ClassReader<T>();
            var allField = ClassReader<T>.ReadEntity();
            foreach (var f in allField)
            {
                var imp = AttributeReader.GetPropertyAttribute<ImportExcel>(typeof(T), f.Name);
                if (imp != null)
                {
                    if (f.DataType.ToLower().Contains("string"))
                    {

                        objmng.SetPropValue(result, f.Name,StringHelper.KillChars(row[imp.ColumnIndex].ToString()));
                    }
                    else if (f.DataType.ToLower().Contains("int"))
                    {
                        try
                        {
                            objmng.SetPropValue(result, f.Name, int.Parse(row[imp.ColumnIndex].ToString()));
                        }
                        catch
                        {

                        }
                    }
                    else if (f.DataType.ToLower().Contains("long"))
                    {
                        try
                        {
                            objmng.SetPropValue(result, f.Name, long.Parse(row[imp.ColumnIndex].ToString()));
                        }
                        catch
                        {

                        }

                    }
                    else if (f.DataType.ToLower().Contains("double"))
                    {
                        try
                        {
                            objmng.SetPropValue(result, f.Name, double.Parse(row[imp.ColumnIndex].ToString()));
                        }
                        catch
                        {

                        }

                    }
                    else if (f.DataType.ToLower().Contains("bool"))
                    {
                        try
                        {
                            if (row[imp.ColumnIndex] == null) objmng.SetPropValue(result, f.Name, false);
                            string value = row[imp.ColumnIndex].ToString().ToLower();
                            bool ketQua = (value == "true" || value == "yes" || value == "có" || value == "x") ? true : false;
                            objmng.SetPropValue(result, f.Name, ketQua);
                        }
                        catch
                        {

                        }

                    }
                    else if (f.DataType.ToLower().Contains("date"))
                    {
                        try
                        {
                            if (string.IsNullOrEmpty(imp.Format))
                            {
                                string tmp = row[imp.ColumnIndex].ToString();
                                if(!tmp.Contains('/'))
                                {
                                    var serialDate = Convert.ToInt32(tmp);
                                    DateTime dt = DateTime.FromOADate(serialDate);
                                    objmng.SetPropValue(result, f.Name, dt);
                                }
                                else
                                {
                                    string strDate = "";
                                    int i = 0;
                                    foreach (string bccm in tmp.Split("/-".ToCharArray(), StringSplitOptions.RemoveEmptyEntries))
                                    {
                                        i++;
                                        if (!string.IsNullOrEmpty(bccm.Trim()))
                                        {
                                            var m = bccm.Trim();
                                            if (i < 3)
                                                strDate += m.PadLeft(2, '0') + "/";
                                            else
                                                strDate += m;
                                        }
                                    }

                                    //row[imp.ColumnIndex].ToString().Split();

                                    //var xxx = row[imp.ColumnIndex].ToString("00/00/0000");
                                    var l = strDate.Length;

                                    string s1 = "dd/MM/yyyy";
                                    string s2 = "MM/dd/yyyy";
                                    if (l < 10)
                                    {
                                        s1 = "dd/MM/yy";
                                        s2 = "MM/dd/yy";
                                    }


                                    DateTime date2;
                                    if (DateTime.TryParseExact(strDate, s1, null, System.Globalization.DateTimeStyles.None, out date2))
                                        objmng.SetPropValue(result, f.Name, date2);
                                    else
                                    {
                                        DateTime date3;
                                        if (DateTime.TryParseExact(strDate, s2, null, System.Globalization.DateTimeStyles.None, out date3))
                                            objmng.SetPropValue(result, f.Name, date2);
                                        else
                                        {
                                            DateTime date;
                                            if (DateTime.TryParse(strDate, out date))
                                                objmng.SetPropValue(result, f.Name, date);
                                        }
                                    }
                                }
                                

                            }
                            else objmng.SetPropValue(result, f.Name, DateTime.ParseExact(row[imp.ColumnIndex].ToString(), imp.Format, null));
                        }
                        catch//(Exception ex)
                        {

                        }
                    }
                }
            }
            objmng.SetPropValue(result, "KichHoat", true);


            return result;
        }
    }
}
