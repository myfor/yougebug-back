using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Administrators
{
    /// <summary>
    /// 
    /// </summary>
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
                    return (false, "账号不能为空");
                if (string.IsNullOrWhiteSpace(Password))
                    return (false, "密码不能为空");
                return (true, "");
            }
        }
    }
}
