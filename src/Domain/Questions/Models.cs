using System.Collections.Generic;

namespace Domain.Questions
{
    public class Models
    {
        /// <summary>
        /// 客户端的问题列表
        /// </summary>
        public struct QuentionItem_Client
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
            /// <summary>
            /// 赞同数
            /// </summary>
            public int VoteCounts { get; set; }
            /// <summary>
            /// 浏览数
            /// </summary>
            public int ViewCounts { get; set; }
            /// <summary>
            /// 回答数
            /// </summary>
            public int AnswerCounts { get; set; }
            /// <summary>
            /// 标签
            /// </summary>
            public string[] Tags { get; set; }
            /// <summary>
            /// 提问人
            /// </summary>
            public string AskerName { get; set; }
            /// <summary>
            /// 提问人头像
            /// </summary>
            public string AskerAvatar { get; set; }
        }
        /// <summary>
        /// 管理端问题列表单项
        /// </summary>
        public struct QuestionItem_Admin
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }
        /// <summary>
        /// 问题的详情
        /// </summary>
        public struct QuestionDetail
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string[] Tags { get; set; }
            public int Votes { get; set; }
            public int Views { get; set; }
            public string Actived { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
            //  public List<Answers.Models.AnswerItem> Answers { get; set; }
            public Paginator Page { get; set; }
        }
        /// <summary>
        /// 提一个提问要的参数
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
        /// <summary>
        /// 举报一个问题需要的参数
        /// </summary>
        public struct NewReport
        {
            public string Reason { get; set; }
            public string Description { get; set; }
        }
    }
}
