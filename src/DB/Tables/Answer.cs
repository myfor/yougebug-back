using System.Collections.Generic;
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
        public int? AnswererId { get; set; } = null;
        /// <summary>
        /// 回答者
        /// </summary>
        public User Answerer { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public List<AnswerBackRecord> AnswerBackRecords { get; set; }
        public List<AnswerReportRecord> AnswerReportRecords { get; set; }
        /// <summary>
        /// 昵称
        /// </summary>
        [Required, StringLength(64)]
        public string NickName { get; set; } = "";
    }
}
