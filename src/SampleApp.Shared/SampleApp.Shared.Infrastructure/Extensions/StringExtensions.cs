namespace SampleApp.Shared.Infrastructure.Extensions
{
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Net;
    using System.Security.Cryptography;
    using System.Text;
    using System.Text.RegularExpressions;

    public static class StringExtensions
    {
        public static string After(this string source, string delimiter)
        {
            var pos = source.IndexOf(delimiter, StringComparison.Ordinal);
            return pos < 0 ? String.Empty : source.Substring(pos + delimiter.Length);
        }

        public static string Before(this string source, string delimiter)
        {
            var pos = source.IndexOf(delimiter, StringComparison.Ordinal);
            return pos < 0 ? String.Empty : source.Substring(0, pos);
        }

        public static string ChopEnd(this string source, int length)
        {
            return source.Substring(0, source.Length - length);
        }

        public static string ChopEnd(this string source, string target)
        {
            return source.EndsWith(target) ? source.ChopEnd(target.Length) : source;
        }

        public static string ChopStart(this string source, int length)
        {
            return source.Substring(length, source.Length - length);
        }

        public static string ChopStart(this string source, string target)
        {
            return source.StartsWith(target) ? source.ChopStart(target.Length) : source;
        }

        public static bool Contains(this string source, string toCheck, bool ignoreCase = false)
        {
            var comp = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;
            return source.IndexOf(toCheck, comp) >= 0;
        }

        public static bool Contains(this string source, string[] matches, bool ignoreCase = false)
        {
            foreach (var match in matches)
            {
                if (source.Contains(match, ignoreCase)) return true;
            }

            return false;
        }

        public static bool ContainsAll(this string source, string[] matches, bool ignoreCase = false)
        {
            foreach (var match in matches)
            {
                if (!source.Contains(match, ignoreCase)) return false;
            }

            return true;
        }

        /// <summary>
        ///     Compute the distance between two strings.
        /// </summary>
        public static int Distance(this string source, string target)
        {
            source ??= "";
            target ??= "";

            var n = source.Length;
            var m = target.Length;
            var d = new int[n + 1, m + 1];

            // Step 1
            if (n == 0) return m;
            if (m == 0) return n;

            // Step 2
            for (var i = 0; i <= n; d[i, 0] = i++)
            {
            }

            for (var j = 0; j <= m; d[0, j] = j++)
            {
            }

            // Step 3
            for (var i = 1; i <= n; i++)
            {
                //Step 4
                for (var j = 1; j <= m; j++)
                {
                    // Step 5
                    var cost = target[j - 1] == source[i - 1] ? 0 : 1;

                    // Step 6
                    d[i, j] = Math.Min(
                        Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1),
                        d[i - 1, j - 1] + cost);
                }
            }

            // Step 7
            return d[n, m];
        }

        public static bool EndsWith(this string source, string[] matches, bool ignoreCase = false)
        {
            var comp = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            foreach (var match in matches)
            {
                if (source.EndsWith(match, comp)) return true;
            }

            return false;
        }

        public static bool EqualsInsensitive(this string source, string target)
        {
            return String.Equals(source, target, StringComparison.CurrentCultureIgnoreCase);
        }

        public static string HtmlDecode(this string source)
        {
            return WebUtility.HtmlDecode(source);
        }

        public static string HtmlEncode(this string source)
        {
            return WebUtility.HtmlEncode(source);
        }

        public static string HtmlFormat(this string source)
        {
            return source.Replace("\r\n", "<br />").Replace("\n", "<br />");
        }

        public static string Md5(this string source)
        {
            var md5 = new MD5CryptoServiceProvider();
            var newdata = Encoding.Default.GetBytes(source);
            var encrypted = md5.ComputeHash(newdata);
            return BitConverter.ToString(encrypted).Replace("-", String.Empty).ToLower();
        }

        public static long Md5AsInt(this string source)
        {
            var md5 = new MD5CryptoServiceProvider();
            var newdata = Encoding.Default.GetBytes(source);
            var encrypted = md5.ComputeHash(newdata);
            return BitConverter.ToInt64(encrypted, 0);
        }

        public static IDictionary<string, string> ParseSeparatedValue(this string source)
        {
            return source.ParseSeparatedValue(";", "=");
        }

        public static IDictionary<string, string> ParseSeparatedValue(this string source, string keyDelimiter, string valueDelimiter)
        {
            return source
                .Split(keyDelimiter)
                .ToDictionary(
                    _ => _.Before(valueDelimiter),
                    _ =>
                    {
                        var val = _.After(valueDelimiter);
                        return string.IsNullOrEmpty(val) ? "" : val;
                    }
                );
        }

        public static string RemoveDiacritics(this string source)
        {
            var tempBytes = Encoding.GetEncoding("ISO-8859-8").GetBytes(source);
            return Encoding.UTF8.GetString(tempBytes);

            // Alternate implementation
            //if (str == null) return null;
            //var chars =
            //    from c in str.Normalize(NormalizationForm.FormD).ToCharArray()
            //    let uc = CharUnicodeInfo.GetUnicodeCategory(c)
            //    where uc != UnicodeCategory.NonSpacingMark
            //    select c;

            //var cleanStr = new string(chars.ToArray()).Normalize(NormalizationForm.FormC);

            //return cleanStr;
        }

        public static string Repeat(this string source, int n)
        {
            var result = String.Empty;

            for (var i = 0; i < n; i++)
            {
                result += source;
            }

            return result;
        }

        //public static string Replace(this string s, string pattern, string replacement, StringComparison comparisonType)
        //{
        //	if (s == null) return null;
        //	if (string.IsNullOrEmpty(pattern)) return s;

        //	var lenPattern = pattern.Length;
        //	var idxPattern = -1;
        //	var idxLast = 0;

        //	var result = new StringBuilder();

        //	while (true)
        //	{
        //		idxPattern = s.IndexOf(pattern, idxPattern + 1, comparisonType);
        //		if (idxPattern < 0)
        //		{
        //			result.Append(s, idxLast, s.Length - idxLast);
        //			break;
        //		}

        //		result.Append(s, idxLast, idxPattern - idxLast);
        //		result.Append(replacement);
        //		idxLast = idxPattern + lenPattern;
        //	}

        //	return result.ToString();
        //}

        public static string[] Split(this string source, string delimiter)
        {
            return source.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
        }

        public static bool StartsWith(this string source, string[] matches, bool ignoreCase = false)
        {
            var comp = ignoreCase ? StringComparison.CurrentCultureIgnoreCase : StringComparison.CurrentCulture;

            foreach (var match in matches)
            {
                if (source.StartsWith(match, comp)) return true;
            }

            return false;
        }

        public static string[] ToArray(this string source, string delimiter)
        {
            return source.Split(delimiter);
        }

        public static List<string> ToList(this string cource, string delimiter)
        {
            return new List<string>(
                cource.Split(delimiter.ToCharArray(), StringSplitOptions.RemoveEmptyEntries));
        }

        public static string ToSlug(this string source)
        {
            if (String.IsNullOrEmpty(source)) return "-";

            var str = source.ToLower().RemoveDiacritics();
            str = Regex.Replace(str, @"[^a-z0-9\s-]", ""); // remove punctuation and non-ascii characters
            str = Regex.Replace(str, @"\s+", " ").Trim(); // convert multiple spaces into one space
            str = str.Truncate(45);
            str = Regex.Replace(str, " - ", " ");
            str = Regex.Replace(str, @"\s", "-"); // replace spaces with hyphens
            return String.IsNullOrEmpty(str) ? "-" : str;
        }

        public static Stream ToStream(this string s)
        {
            return s.ToStream(Encoding.UTF8);
        }

        public static Stream ToStream(this string s, Encoding encoding)
        {
            return new MemoryStream(encoding.GetBytes(s ?? ""));
        }

        public static string ToTitleCase(this string source)
        {
            if (String.IsNullOrEmpty(source))
            {
                return String.Empty;
            }

            return Char.ToUpper(source[0]) + source.Substring(1);
        }

        public static string Truncate(this string source, int maxLength)
        {
            return source.Length <= maxLength ? source : source.Substring(0, maxLength);
        }

        public static string UrlDecode(this string source)
        {
            return WebUtility.UrlDecode(source);
        }

        public static string UrlEncode(this string souce)
        {
            return WebUtility.UrlEncode(souce);
        }

        //private static byte[] DecodeBase64(this string source, Encoding encoding = null)
        //{
        //    return Convert.FromBase64String(source);
        //}
    }
}
