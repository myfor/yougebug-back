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
            public string Name { get; set; }
            [NonSerialized]
            public Guid Token;
            [NonSerialized]
            public int Id;
        }
    }
}
