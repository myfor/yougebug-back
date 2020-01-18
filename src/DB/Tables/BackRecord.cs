using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 退回记录
    /// </summary>
    public class BackRecord : Entity
    {
        public int QuestionId { get; set; }
        /// <summary>
        /// 描述
        /// </summary>
        [Required, StringLength(1024)]
        public string Description { get; set; } = "";
    }
}
