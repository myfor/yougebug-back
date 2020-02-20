using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 回答的退回记录
    /// </summary>
    public class AnswerBackRecord : Entity
    {
        public int AnswerId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required, StringLength(1024)]
        public string Description { get; set; } = "";
    }
}
