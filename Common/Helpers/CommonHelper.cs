using System;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Common.Helpers
{
    public partial class CommonHelper
    {
        public static string MD5Hash(string input)
        {
            using (var md5 = MD5.Create())
            {
                var result = md5.ComputeHash(Encoding.ASCII.GetBytes(input));
                return Encoding.ASCII.GetString(result);
            }
        }
        public static string StringToSHA512(string inputString)
        {
            SHA512 sha512 = SHA512Managed.Create();
            byte[] bytes = Encoding.UTF8.GetBytes(inputString);
            byte[] hash = sha512.ComputeHash(bytes);
            return GetStringFromHash(hash);
        }
        private static string GetStringFromHash(byte[] hash)
        {
            var s = string.Empty;
            return hash.Aggregate(s, (current, b) => current + b.ToString("X5"));
        }
        public static string ConvertStringtoMD5(string strword)
        {
            MD5 md5 = MD5.Create();
            byte[] inputBytes =Encoding.ASCII.GetBytes(strword);
            byte[] hash = md5.ComputeHash(inputBytes);
            StringBuilder sb = new();
            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("x2"));
            }
            return sb.ToString().ToUpper();
        }
        public static string CalculateHash(string input)
        {
            using var algorithm = SHA512.Create(); //or MD5 SHA256 etc.
            var hashedBytes = algorithm.ComputeHash(Encoding.UTF8.GetBytes(input));

            return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
        }
        public static string ToURL(string unicodeString, int length = 10)
        {
            if (length > 0)
                return RemoveSpecialCharacters(RemoveMarks(unicodeString)).Replace(" ", "-").Replace("--", "-") + "-" +
                       RandomString(length);
            return RemoveSpecialCharacters(RemoveMarks(unicodeString)).Replace(" ", "-").Replace("--", "-");
        }
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
        public static string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
                return string.Empty;

            var r = new Regex("(?:[^a-z0-9 -]|(?<=['\"])s)", RegexOptions.IgnoreCase | RegexOptions.CultureInvariant | RegexOptions.Compiled);
            return r.Replace(input, String.Empty);
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
    }
}
