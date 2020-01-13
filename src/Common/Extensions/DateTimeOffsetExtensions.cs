namespace System
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
    }
}
