﻿namespace System
{
    public static class DateTimeOffsetExtensions
    {
        /// <summary>
        /// 转换成 HH:mm 的时间格式
        /// </summary>
        public static string ToStandardTimeString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("HH:mm");
        }

        /// <summary>
        /// 转换成 yyyy-MM-dd HH:mm 的时间格式
        /// </summary>
        public static string ToStandardString(this DateTimeOffset dateTimeOffset)
        {
            return dateTimeOffset.ToString("yyyy-MM-dd HH:mm");
        }
    }
}
