using System.ComponentModel;
using System.Reflection;

namespace System
{
    /// <summary>
	/// 枚举扩展方法
	/// </summary>
	public static class EnumExtensions
    {
        /// <summary>
        /// 获取枚举的 Description 内容
        /// </summary>
        public static string GetDescription(this Enum @enum)
        {
            Type type = @enum.GetType();
            MemberInfo[] memInfo = type.GetMember(@enum.ToString());
            if (null != memInfo && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (null != attrs && attrs.Length > 0)
                    return ((DescriptionAttribute)attrs[0]).Description;
            }
            return @enum.ToString();
        }
    }
}
