using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Domain.Clients
{
    public class Models
    {
        public struct LoginInfo
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
                    return (false, "登录账号不能为空");
                if (string.IsNullOrWhiteSpace(Password))
                    return (false, "密码不能为空");
                return (true, "");
            }
        }

        public struct UserIntro
        {
            public int Id { get; set; }
            public string Account { get; set; }
            public string Avatar { get; set; }
        }
        /// <summary>
        /// 用户注册
        /// </summary>
        public struct RegisterInfo
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
    }
}
