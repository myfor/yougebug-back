using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Answer : Entity
    {
        /// <summary>
        /// 答案
        /// </summary>
        [Required]
        public string Content { get; set; } = "";
        public User Creator { get; set; }
    }
}
