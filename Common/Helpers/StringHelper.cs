using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public static class StringHelper
    {
        public static string KillChars(string strInput)
        {
            string result = "";
            if (!String.IsNullOrEmpty(strInput))
            {
                string[] arrBadChars = new string[] { "select", "drop", "--", "insert", "delete", "xp_", "sysobjects", "syscolumns", "'", "1=1", "truncate", };//Loại bỏ "or, table" để tránh lỗi không nhập được những từ có chứa "or, table"
                result = strInput.Trim().Replace("'", "''");
                result = result.Replace("%20", " ");
                for (int i = 0; i < arrBadChars.Length; i++)
                {
                    string strBadChar = arrBadChars[i].ToString();
                    result = result.Replace(strBadChar, "");
                }
            }
            return result;
        }
        public static string CreateRandomString(int len)
        {
            string _allowedChars = "abcdefghijk0123456789mnopqrstuvwxyz";
            Random randNum = new Random();
            char[] chars = new char[len];
            int allowedCharCount = _allowedChars.Length;
            for (int i = 0; i < len; i++)
            {
                chars[i] = _allowedChars[(int)((_allowedChars.Length) * randNum.NextDouble())];
            }
            return new string(chars);
        }
        public static string ToURL(string unicodeString, int length = 10)
        {
            if (length > 0)
                return RemoveSpecialCharacters(RemoveMarks(unicodeString)).Replace(" ", "-").Replace("--", "-") + "-" +
                       RandomString(length);
            return RemoveSpecialCharacters(RemoveMarks(unicodeString)).Replace(" ", "-").Replace("--", "-");
        }
        public static string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var r = new Regex("(?:[^a-z0-9 -]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
        }
        /// <summary>
        /// Hàm tạo 1 chuỗi ngẫu nhiên các ký tự có độ dài length
        /// </summary>
        /// <param name="length"></param>
        /// <returns></returns>
        public static string RandomString(int length)
        {
            string outValue = string.Empty;
            char[] charArray = {
                                   'a', 'q', 'w', 'e', 'r', 't', 'y', 'u', 'i', 'o', 'p', 'd', 's', 'f', 'g', 'h', 'j', 'k'
                                   , 'l', 'z', 'x', 'c', 'v', 'b', 'n', 'm', '1', '2', '3', '4', '5', '6', '7', '8', '9',
                                   '0'
                               };
            var r = new Random();
            for (int i = 0; i < length; i++)
            {
                outValue += charArray[r.Next(charArray.Length)];
            }
            return outValue;
        }
        public static string RemoveMarks(string unicodeString)
        {
            try
            {
                //Remove VietNamese character
                unicodeString = unicodeString.ToLower();
                unicodeString = Regex.Replace(unicodeString, "[áàảãạâấầẩẫậăắằẳẵặ]", "a");
                unicodeString = Regex.Replace(unicodeString, "[éèẻẽẹêếềểễệ]", "e");
                unicodeString = Regex.Replace(unicodeString, "[iíìỉĩị]", "i");
                unicodeString = Regex.Replace(unicodeString, "[óòỏõọơớờởỡợôốồổỗộ]", "o");
                unicodeString = Regex.Replace(unicodeString, "[úùủũụưứừửữự]", "u");
                unicodeString = Regex.Replace(unicodeString, "[yýỳỷỹỵ]", "y");
                unicodeString = Regex.Replace(unicodeString, "[đ]", "d");

                //Remove space
                unicodeString = StandardSpace(unicodeString);
                unicodeString = unicodeString.TrimEnd().TrimStart();

                return unicodeString;
            }
            catch
            {
                return string.Empty;
            }
        }
        public static string StandardSpace(string value)
        {
            if (value != null)
                value = Regex.Replace(value, @"\s+", " ");
            if (value != null)
                value = value.TrimEnd().TrimStart();
            return value;
        }
        public static MemoryStream GenerateStreamFromString(string value)
        {
            return new MemoryStream(Encoding.UTF8.GetBytes(value ?? ""));
        }
        public static string RomanNumeralFrom(int number)
        {
            return
                new string('I', number)
                    .Replace(new string('I', 1000), "M")
                    .Replace(new string('I', 900), "CM")
                    .Replace(new string('I', 500), "D")
                    .Replace(new string('I', 400), "CD")
                    .Replace(new string('I', 100), "C")
                    .Replace(new string('I', 90), "XC")
                    .Replace(new string('I', 50), "L")
                    .Replace(new string('I', 40), "XL")
                    .Replace(new string('I', 10), "X")
                    .Replace(new string('I', 9), "IX")
                    .Replace(new string('I', 5), "V")
                    .Replace(new string('I', 4), "IV");
        }
    }
}
