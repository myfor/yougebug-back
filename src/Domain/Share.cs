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
        /// 给只需要放一个参数的使用
        /// </summary>
        public class SingleContent
        {
            public string Content { get; set; }
        }

        public class KeyValue<KT, VT>
        {
            public static KeyValue<K, V> Create<K, V>(K key, V value)
            {
                return new KeyValue<K, V>
                {
                    Key = key,
                    Value = value
                };
            }
            public KT Key { get; set; }
            public VT Value { get; set; }
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
