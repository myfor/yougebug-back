namespace System
{
    public static class Int32Extensions
    {
        /// <summary>
        /// 转换为指定枚举类型并获取 Description
        /// </summary>
        public static string GetDescription<TEnum>(this int value) where TEnum: Enum
        {
            return ((TEnum)(object)value).GetDescription();
        }
    }
}
