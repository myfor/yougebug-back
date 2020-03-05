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
    }
}
