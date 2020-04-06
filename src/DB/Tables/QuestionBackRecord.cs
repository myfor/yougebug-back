using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 提问的退回记录
    /// </summary>
    public class QuestionBackRecord : Entity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required, StringLength(1024)]
        public string Description { get; set; } = "";
    }
}
