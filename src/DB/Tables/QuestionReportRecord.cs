using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 问题举报记录
    /// </summary>
    public class QuestionReportRecord : Entity
    {
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
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
