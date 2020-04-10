using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    /// <summary>
    /// 提问的追问
    /// </summary>
    public class QuestionComment : Entity
    {
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        [Required, StringLength(1024)]
        public string Content { get; set; } = "";
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
    }
}
