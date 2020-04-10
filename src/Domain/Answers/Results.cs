﻿namespace Domain.Answers
{
    public class Results
    {
        /// <summary>
        /// 答案单项
        /// </summary>
        public class AnswerItem
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public int Votes { get; set; }
            public string CreateDate { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }

        /// <summary>
        /// 所有答案列表的单项，只在管理员后台用
        /// </summary>
        public class AnswerItem_All
        {
            public int Id { get; set; }
            /// <summary>
            /// 所属的提问
            /// </summary>
            public string QuestionTitle { get; set; }
            public string Content { get; set; }
            public int Votes { get; set; }
            public string CreateDate { get; set; }
            public string AnswererName { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }
    }
}