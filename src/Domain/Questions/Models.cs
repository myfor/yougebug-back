using System;

namespace Domain.Questions
{
    public class Models
    {
        
        /// <summary>
        /// 提一个提问要的参数
        /// </summary>
        public class PostQuestion
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

                Tags = Tags.ReSplit(',', '，');

                return (true, "");
            }
        }

        /// <summary>
        /// 修改一个提问要的参数
        /// </summary>
        public class EditQuestion
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
            public int CurrentUserId { get; set; }

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

                Tags = Tags.ReSplit(',', '，');

                return (true, "");
            }
        }

        /// <summary>
        /// 举报一个问题需要的参数
        /// </summary>
        public class NewReport
        {
            public string Reason { get; set; }
            public string Description { get; set; }
        }

        /// <summary>
        /// 新的追问
        /// </summary>
        public class NewComment
        {
            public string Comment { get; set; }
        }
    }
}
