using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace DB.Tables
{
    public class Question : Entity
    {
        /// <summary>
        /// 标题
        /// </summary>
        [Required, StringLength(256)]
        public string Title { get; set; } = "";
        /// <summary>
        /// 描述
        /// </summary>
        [Required]
        public string Description { get; set; } = "";
        /// <summary>
        /// 标签
        /// </summary>
        [Required, StringLength(64)]
        public string Tags { get; set; } = "";
        /// <summary>
        /// 点赞数
        /// </summary>
        [Required]
        public int Votes { get; set; } = 0;
        /// <summary>
        /// 浏览人数
        /// </summary>
        [Required]
        public int Views { get; set; } = 0;
        /// <summary>
        /// 最近活跃时间
        /// </summary>
        [Required]
        public DateTimeOffset Actived { get; set; } = DateTimeOffset.Now;
        /// <summary>
        /// 提问人
        /// </summary>
        public User Asker { get; set; }
        public int AskerId { get; set; }
        /// <summary>
        /// 退回记录
        /// </summary>
        public List<QuestionBackRecord> QuestionBackRecords { get; set; }
        /// <summary>
        /// 回答列表
        /// </summary>
        public List<Answer> Answers { get; set; }
        public List<QuestionReportRecord> QuestionReportRecords { get; set; }
    }
}
