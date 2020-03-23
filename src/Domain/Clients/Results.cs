using System;

namespace Domain.Clients
{
    public class Results
    {
        /// <summary>
        /// 登录结果信息
        /// </summary>
        public struct LoggedInInfo
        {
            public string Avatar { get; set; }
            public string Name { get; set; }
            /// <summary>
            /// JWT，不序列化
            /// </summary>
            [NonSerialized]
            public Guid Token;
            /// <summary>
            /// ID，不序列化
            /// </summary>
            [NonSerialized]
            public int Id;
        }

        /// <summary>
        /// 客户单项
        /// </summary>
        public struct ClientItem
        {
            public int Id { get; set; }
            public string UserName { get; set; }
            public string Email { get; set; }
            public string CreateDate { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }

        public struct ClientDetail
        {
            public string UserName { get; set; }
            public string Email { get; set; }
            public string CreateDate { get; set; }
            public string Avatar { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }
    }
}
