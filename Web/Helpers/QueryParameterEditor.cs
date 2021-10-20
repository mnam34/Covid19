using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Web.Helpers
{
    /*
     * Cách dùng
      string originalUrl = "https://somehost.org/somepath/somepage?someparam=test#soem fragment"; 
 
            var qpe = new QueryParameterEditor(originalUrl); 
 
            qpe["page"] = "1"; 
            qpe.SetQueryParam("page", "2").RemoveQueryParam("someparam"); 
 
            ViewBag.OriginalUrl = originalUrl; 
            ViewBag.EditedUrl = qpe.ToString(); 

    */
    public static class Extension
    {
        public static void CutStringByFirstKey(this string sourceString, string key, out string str1, out string str2)
        {
            int index = sourceString.IndexOf(key);

            if (index == -1)
            {
                str1 = sourceString;
                str2 = string.Empty;
            }
            else
            {
                str1 = sourceString.Substring(0, index);
                str2 = sourceString.Substring(index + 1);
            }
        }
    }
    public class QueryParameterEditor
    {
        private List<QueryParameter> _querys = new List<QueryParameter>();

        public string Authority { get; set; }
        public string QueryString
        {
            get
            {
                return string.Join("&", _querys);
            }
        }

        public string Fragment { get; set; }

        public string[] AllKeys
        {
            get
            {
                return _querys.Select(m => m.Key).ToArray();
            }
        }

        public string this[string key]
        {
            get
            {
                var param = _querys.FirstOrDefault(m => m.Key.Equals(key, StringComparison.CurrentCultureIgnoreCase));
                return param == null ? null : param.Value;
            }
            set { SetQueryParam(key, value); }
        }

        public QueryParameterEditor(string uriString)
        {
            string authority, queryString, fragment;
            uriString.CutStringByFirstKey("?", out authority, out queryString);

            queryString.CutStringByFirstKey("#", out queryString, out fragment);

            Authority = authority;
            InitParameters(queryString);
            Fragment = fragment;
        }

        public QueryParameterEditor SetQueryParam(string key, string value)
        {
            var param = new QueryParameter { Key = key, Value = value };

            SetQueryParam(param);

            return this;
        }

        public QueryParameterEditor SetQueryParam(params QueryParameter[] parameters)
        {
            foreach (var item in parameters)
            {
                var queryParam = _querys.FirstOrDefault(m => m.Key.Equals(item.Key, StringComparison.CurrentCultureIgnoreCase));
                if (queryParam == null)
                {
                    _querys.Add(item);
                }
                else
                {
                    queryParam.Value = item.Value;
                }
            }

            return this;
        }

        public QueryParameterEditor RemoveQueryParam(params string[] keys)
        {
            foreach (var key in keys)
            {
                var queryParam = _querys.FirstOrDefault(m => m.Key == key);
                _querys.Remove(queryParam);
            }
            return this;
        }

        public override string ToString()
        {
            return Authority +
                (string.IsNullOrEmpty(QueryString) ? "" : $"?{QueryString}") +
                (string.IsNullOrEmpty(Fragment) ? "" : $"#{Fragment}");
        }

        private void InitParameters(string queryString)
        {
            string[] queryArr = queryString.Split('&');
            foreach (var item in queryArr)
            {
                if (!string.IsNullOrEmpty(item))
                {
                    string key, value;
                    item.CutStringByFirstKey("=", out key, out value);
                    _querys.Add(new QueryParameter { Key = key, Value = value });
                }
            }
        }
    }

    public class QueryParameter
    {
        public string Key { get; set; }

        public string Value { get; set; }

        public override string ToString()
        {
            return $"{Key}={Value}";
        }
    }
}
