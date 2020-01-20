using System;
using System.Collections.Generic;
using System.Text;

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
                    return (false, "账号不能为空");
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
    }
}
