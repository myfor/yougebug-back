using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 答案举报记录
    /// </summary>
    public class AnswerReportRecord : Entity
    {
        [Required]
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        /// <summary>
        /// 举报原因
        /// </summary>
        [Required, StringLength(256)]
        public string Content { get; set; } = "";
        public bool IsHandled { get; set; } = false;
        /// <summary>
        /// 描述
        /// </summary>
        [Required, StringLength(64)]
        public string Description { get; set; } = "";
    }
}
