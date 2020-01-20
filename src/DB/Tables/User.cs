using System;
using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class User : Entity
    {
        public string Name { get; set; } = "";
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
        public Guid Token { get; set; }
        /// <summary>
        /// 头像
        /// </summary>
        [Required]
        public int AvatarId { get; set; }
        public File Avatar { get; set; }
    }
}
