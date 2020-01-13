using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class User : Entity
    {
        /// <summary>
        /// 邮箱，也是登录账号
        /// </summary>
        [Required, StringLength(64)]
        public string Email { get; set; } = "";
        /// <summary>
        /// 密码
        /// </summary>
        [Required, StringLength(64)]
        public string Password { get; set; } = "";
        public List<Question> Questions { get; set; }
        public List<Answer> Answers { get; set; }
    }
}
