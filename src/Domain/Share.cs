using System.Collections.Generic;

namespace Domain
{
    /// <summary>
    /// 共享
    /// </summary>
    public class Share
    {
        #region 枚举
        
        public enum Platform
        {
            /// <summary>
            /// 管理惯
            /// </summary>
            Admin,
            /// <summary>
            /// 客户端
            /// </summary>
            Client
        }

        #endregion

        #region 模型

        /// <summary>
        /// 键值对模型
        /// </summary>
        public class KeyValue<K, V>
        {
            public K Key { get; set; }
            public V Value { get; set; }
        }

        public class KeyValueChlid<K, V>
        {
            public K Key { get; set; }
            public V Value { get; set; }
            public List<KeyValueChlid<K, V>> Child { get; set; }
        }

        public struct Image
        {
            /// <summary>
            /// 缩略图
            /// </summary>
            public string Thumbnail { get; set; }
            /// <summary>
            /// 原图
            /// </summary>
            public string Source { get; set; }
        }
        #endregion
    }
}
