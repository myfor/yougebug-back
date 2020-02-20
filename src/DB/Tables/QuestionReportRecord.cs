﻿using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    /// <summary>
    /// 问题举报记录
    /// </summary>
    public class QuestionReportRecord
    {
        [Required]
        public int QuestionId { get; set; }
        public Question Question { get; set; }
        /// <summary>
        /// 举报原因
        /// </summary>
        [Required, StringLength(64)]
        public string Reason { get; set; } = "";
        /// <summary>
        /// 描述
        /// </summary>
        [Required, StringLength(64)]
        public string Description { get; set; } = "";
    }
}
