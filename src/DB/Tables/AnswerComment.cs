using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace DB.Tables
{
    /// <summary>
    /// 回答的追问
    /// </summary>
    public class AnswerComment : Entity
    {
        public int AnswerId { get; set; }
        public Answer Answer { get; set; }
        [Required, StringLength(1024)]
        public string Content { get; set; } = "";
        public int CommenterId { get; set; }
        public User Commenter { get; set; }
    }
}
