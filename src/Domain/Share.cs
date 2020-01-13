using System.ComponentModel;
using System.Threading.Tasks;

namespace Domain
{
    /// <summary>
    /// 共享
    /// </summary>
    public class Share
    {
        #region 枚举
        

        #endregion

        #region 模型

        /// <summary>
        /// 键值对模型
        /// </summary>
        public class KeyValue
        {
            public int Key { get; set; } = 0;
            public string Value { get; set; } = "";
        }

        /// <summary>
        /// 泛型键值对模型
        /// </summary>
        public class KeyValue<K, V>
        {
            public K Key { get; set; }
            public V Value { get; set; }
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
