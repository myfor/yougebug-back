using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 回答
    /// </summary>
    public class Answer : Entity
    {
        /// <summary>
        /// 答案
        /// </summary>
        [Required]
        public string Content { get; set; } = "";
        public int Votes { get; set; } = 0;
        public User Creator { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
    }
}
