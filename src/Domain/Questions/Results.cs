using System;
using System.Collections.Generic;
using System.Text;

namespace Domain.Questions
{
    public class Results
    {
        /// <summary>
        /// 客户端的问题列表
        /// </summary>
        public class QuentionItem_Client
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
        public class QuestionItem_Admin
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
            public int AnswersCount { get; set; }
        }
        /// <summary>
        /// 获取用户详情下的提问列表
        /// </summary>
        public class QuestionItem_UserSelf
        {
            public int Id { get; set; }
            /// <summary>
            /// 是否本人
            /// </summary>
            public bool IsSelf { get; set; }
            public string Title { get; set; }
            public string Description { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
            public int AnswersCount { get; set; }
        }

        /// <summary>
        /// 问题的详情
        /// </summary>
        public class QuestionDetail
        {
            /// <summary>
            /// 是否为本人
            /// </summary>
            public bool IsSelf { get; set; }
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
        /// 提问编辑的信息
        /// </summary>
        public class QuestionEditInfo
        {
            public int Id { get; set; }
            public string Title { get; set; }
            public string Content { get; set; }
            public string[] Tags { get; set; }
        }

        /// <summary>
        /// 举报列表
        /// </summary>
        public class ReportItem
        {
            public int QuestionId { get; set; }
            public string Title { get; set; }
            public int ReportCount { get; set; }
        }
    }
}
