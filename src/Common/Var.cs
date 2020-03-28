namespace Common
{
    /// <summary>
    /// 参数
    /// </summary>
    public class Var
    {
        private static Var _var = new Var();
        public static Var GetVar() => _var;
        /// <summary>
        /// 用户名最短长度
        /// </summary>
        public int UserNameMinLength { get; set; } = 2;
        /// <summary>
        /// 不允许的用户名
        /// </summary>
        public string[] NonAllowedUserName { get; set; } = { };
        /// <summary>
        /// 用户名不允许包含的字符
        /// </summary>
        public string[] NonAllowedContainCharacter { get; set; } = { };
    }
}
