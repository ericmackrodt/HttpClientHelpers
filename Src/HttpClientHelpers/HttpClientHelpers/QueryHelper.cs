using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace HttpClientHelpers
{
    public static class QueryHelper
    {
        public static string BuildQueryString(this object queryObject)
        {
            var queryString = "";

            if (queryObject == null) return "";

            var type = queryObject.GetType();
            var properties = RuntimeReflectionExtensions.GetRuntimeProperties(type);

            if (properties != null && properties.Any())
            {
                var q = properties.Where(o => o.GetValue(queryObject) != null).Select(o => string.Concat(o.Name, "=", o.GetValue(queryObject)));
                queryString = string.Join("&", q);
            }

            return queryString;
        }

        public static Dictionary<string, object> GetQueryDictionary(this object parameters)
        {
            if (parameters == null)
                return new Dictionary<string, object>();

            var type = parameters.GetType();
            var properties = RuntimeReflectionExtensions.GetRuntimeProperties(type);

            var dict = new Dictionary<string, object>();

            if (properties != null && properties.Any())
            {
                foreach (var o in properties)
                    dict.Add(o.Name, o.GetValue(parameters));
            }

            return dict;
        }

        public static string BuildRequestUrl(string path, string queryString)
        {
            var sb = new StringBuilder(path);

            if (!string.IsNullOrWhiteSpace(queryString))
            {
                if (!queryString.StartsWith("?"))
                    sb.Append("?");

                sb.Append(queryString);
            }

            return sb.ToString();
        }

        public static string BuildRequestUrl(string baseUri, string path, string queryString)
        {
            var sb = new StringBuilder(baseUri);

            if (!baseUri.EndsWith("/") && path.StartsWith("/")) sb.Append("/");

            sb.Append(BuildRequestUrl(path, queryString));

            return sb.ToString();
        }

        public static Dictionary<string, string> ParseQueryString(this string uri)
        {
            string substring = uri.Substring(((uri.LastIndexOf('?') == -1) ? 0 : uri.LastIndexOf('?') + 1));

            string[] pairs = substring.Split('&');

            Dictionary<string, string> output = new Dictionary<string, string>();

            foreach (string piece in pairs)
            {
                string[] pair = piece.Split('=');
                output.Add(pair[0], pair[1]);
            }

            return output;
        }

        public static string EscapeJson(this string stringToEscape)
        {
            return Regex.Replace(stringToEscape, @"(?<!\\)\\(?!"")(?!n)(?!\\)", @"\\", RegexOptions.IgnorePatternWhitespace);
        }

        public static string EscapeQuery(this string q)
        {
            return string.Join("", q.ToCharArray().Where(o => char.IsLetterOrDigit(o) || char.IsWhiteSpace(o)).Select(o => o.ToString()));
        }
    }
}
