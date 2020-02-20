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
        /// <summary>
        /// 回答者
        /// </summary>
        public User Creator { get; set; }
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        public List<AnswerBackRecord> AnswerBackRecords { get; set; }
        public List<AnswerReportRecord> AnswerReportRecords { get; set; }
    }
}
