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
            public string CreateDate { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }

        /// <summary>
        /// 所有答案列表的单项，只在管理员后台用
        /// </summary>
        public struct AnswerItem_All
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
        /// 新的答案
        /// </summary>
        public struct PostAnswer
        {
            public string Content { get; set; }
            public string NickName { get; set; }
            public bool IsLogin { get; set; }
        }
    }
}
