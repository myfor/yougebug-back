using System;
using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Admin : Entity
    {
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(32)]
        public string Account { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [Required, StringLength(64)]
        public string Password { get; set; } = "";
        /// <summary>
        /// 
        /// </summary>
        [Required]
        public Guid Token { get; set; }
        public string Email { get; set; }
    }
}
