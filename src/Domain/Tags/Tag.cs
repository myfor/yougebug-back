using Common;
using DB;
using System.Linq;

namespace Domain.Tags
{
    /// <summary>
    /// 标签
    /// </summary>
    public class Tag
    {
        /// <summary>
        /// 标签最大长度
        /// </summary>
        public const int TAG_MAX_LENGTH = 32;

        /// <summary>
        /// 该标签是否已存在
        /// </summary>
        /// <param name="tag"></param>
        /// <returns></returns>
        public static bool IsExistTag(string tag)
        {
            bool? exist = Cache.Get<bool?>(tag);
            if (exist is null)
            {
                using var db = new YGBContext();
                exist = db.Tags.Any(t => t.Name == tag);
                Cache.Set(tag, exist);
            }
            return exist.Value;
        }
    }
}
