namespace Domain.Answers
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
            public Clients.Results.UserIntro User { get; set; }
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

        /// <summary>
        /// 管理者側の回答の細部
        /// </summary>
        public class AnswerDetailForAdmin
        {
            public string QuestionTitle { get; set; } = "";
            public string QuestionContent { get; set; } = "";
            public string AnswerContent { get; set; } = "";
            public Share.KeyValue<int, string> State { get; set; }
            public Clients.Results.UserIntro User { get; set; }
            public string CreateDate { get; set; } = "";
        }

        /// <summary>
        /// 管理者側の回答の細部
        /// </summary>
        public class AnswerDetailForClient : AnswerDetailForAdmin
        {
            public bool IsSelf { get; set; }
        }
    }
}
