using System.Collections.Generic;
using System.Text;

namespace System
{
    public static class StringExtensions
    {
        /// <summary>
        /// 将字符串中能转化数字的值分割成数组
        /// </summary>
        public static IList<int> SplitToInt(this string value, char c)
        {
            if (string.IsNullOrWhiteSpace(value))
                return new int[0];

            string[] v = value.Split(c);
            List<int> list = new List<int>(v.Length);
            foreach (string _v in v)
            {
                if (!int.TryParse(_v, out int v_int))
                    continue;
                list.Add(v_int);
            }
            return list;
        }

        /// <summary>
        /// 将字符串根据字符切割成字符串数组
        /// </summary>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] SplitOfChar(this string value, char separator)
        {
            return value.Split(separator);
        }

        /// <summary>
        /// 在原来数组上重新分割
        /// </summary>
        /// <param name="value"></param>
        /// <param name="separator"></param>
        /// <returns></returns>
        public static string[] ReSplit(this string[] value, params char[] separator)
        {
            if (value.Length == 0)
                return value;
            List<string> result = new List<string>(value.Length);
            foreach (string item in value)
            {
                result.AddRange(item.Trim().Split(separator, StringSplitOptions.RemoveEmptyEntries));
            }
            return result.ToArray();
        }

        /// <summary>
        /// 只选取部分描述
        /// </summary>
        public static string Overflow(this string value, int count)
        {
            return value.Overflow(count, "...");
        }

        /// <summary>
        /// 只选取部分描述
        /// </summary>
        public static string Overflow(this string value, int count, string suffix)
        {
            if (value.Length <= count)
                return value;

            ReadOnlySpan<char> str = value.AsSpan(0, count);
            StringBuilder result = new StringBuilder(count + 3);
            result.Append(str).Append(suffix);
            return result.ToString();
        }
    }
}
