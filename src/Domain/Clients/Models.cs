using System.Text.RegularExpressions;

namespace Domain.Clients
{
    public class Models
    {
        public class LoginInfo
        {
            public string Account { get; set; }
            public string Password { get; set; }

            /// <summary>
            /// 检查参数是否有效
            /// </summary>
            /// <returns></returns>
            public (bool, string) IsValid()
            {
                if (string.IsNullOrWhiteSpace(Account))
                    return (false, "用户名或邮箱不能为空");
                if (string.IsNullOrWhiteSpace(Password))
                    return (false, "密码不能为空");
                return (true, "");
            }
        }

        public class UserIntro
        {
            public int Id { get; set; }
            public string Account { get; set; }
            public string Avatar { get; set; }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        public class RegisterInfo
        {
            public string Email { get; set; }
            public string Password { get; set; }

            public (bool, string) IsValid()
            {
                Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
                if (!r.IsMatch(Email))
                    return (false, "邮箱格式有误");
                if (string.IsNullOrWhiteSpace(Password))
                    return (false, "密码有误");

                return (true, "");
            }
        }

        /// <summary>
        /// 修改用户信息参数
        /// </summary>
        public class UserModify
        {
            public string UserName { get; set; }
            public string Email { get; set; }

            public (bool, string) IsValid()
            {
                if (string.IsNullOrWhiteSpace(UserName) || UserName.Trim().Length < Common.Config.Var.UserNameMinLength)
                    return (false, $"用户名长度不能小于{Common.Config.Var.UserNameMinLength}位");
                UserName = UserName.Trim();

                foreach (string character in Common.Config.Var.NonAllowedContainCharacter)
                {
                    if (UserName.Contains(character))
                        return (false, $"不能包含不允许的字符：{string.Join(',', Common.Config.Var.NonAllowedContainCharacter)}");
                }
                
                if (Common.Config.NonAllowedUserName.Contains(UserName))
                    return (false, "不能使用这个名字");

                Regex r = new Regex("^\\s*([A-Za-z0-9_-]+(\\.\\w+)*@(\\w+\\.)+\\w{2,5})\\s*$");
                if (!r.IsMatch(Email))
                    return (false, "邮箱格式有误");

                return (true, "");
            }
        }

        /// <summary>
        /// 用户修改密码
        /// </summary>
        public class ChangePassword
        {
            /// <summary>
            /// 旧密码
            /// </summary>
            public string OldPassword { get; set; }
            /// <summary>
            /// 新密码
            /// </summary>
            public string NewPassword { get; set; }
            /// <summary>
            /// 确认新密码
            /// </summary>
            public string Confirm { get; set; }

            public (bool, string) IsValid()
            {
                if (string.IsNullOrWhiteSpace(OldPassword))
                    return (false, $"请输入旧密码");
                if (string.IsNullOrWhiteSpace(NewPassword))
                    return (false, $"请输入新密码");
                if (string.IsNullOrWhiteSpace(Confirm) || Confirm != NewPassword)
                    return (false, $"两次新密码不一致");

                return (true, "");
            }
        }
    }
}
