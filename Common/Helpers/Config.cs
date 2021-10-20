using System;
using System.Collections.Generic;
using System.Text;

namespace Common.Helpers
{
    public class Config
    {
        public static String Md5Key
        {
            get { return CommonHelper.MD5Hash("thotaxationdepartment"); }
        }
        public static String Md5Key2
        {
            get { return CommonHelper.ConvertStringtoMD5("thotaxationdepartment"); }
        }
    }
}
