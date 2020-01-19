using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Answers
{
    public class Models
    {
        /// <summary>
        /// 答案单项
        /// </summary>
        public struct AnswerItem
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public int Votes { get; set; }
        }
    }
}
