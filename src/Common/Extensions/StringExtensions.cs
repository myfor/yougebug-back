using System.Collections.Generic;

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
    }
}
