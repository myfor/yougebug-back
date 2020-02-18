using System.Collections.Generic;

namespace Domain.Questions
{
    public class Models
    {
        public struct QuentionItem_Client
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
        }
        public struct QuestionItem_Admin
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
        }
        /// <summary>
        /// 问题的详情
        /// </summary>
        public struct QuestionDetail
        {
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Tags { get; set; }
            public int Votes { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
            public List<Answers.Models.AnswerItem> Answers { get; set; }
        }
        /// <summary>
        /// 提一个问题要的参数
        /// </summary>
        public struct PostQuestion
        {
            /// <summary>
            /// 问题标题
            /// </summary>
            public string Title { get; set; }
            /// <summary>
            /// 问题描述
            /// </summary>
            public string Description { get; set; }
            /// <summary>
            /// 问题标签
            /// </summary>
            public string[] Tags { get; set; }
            /// <summary>
            /// 提问者 ID
            /// </summary>
            public int UserId { get; set; }

            /// <summary>
            /// 检查参数是否有效
            /// </summary>
            /// <returns></returns>
            public (bool, string) IsValid()
            {
                if (string.IsNullOrWhiteSpace(Title))
                    return (false, "问题标题不能为空");
                if (string.IsNullOrWhiteSpace(Description))
                    return (false, "问题描述不能为空");
                if (Tags.Length <= 0)
                    return (false, "请输入至少一个问题标签");
                return (true, "");
            }
        }
    }
}
