using System.Collections.Generic;
using System;

namespace Domain
{
    public class Paginator
    {
        public static Paginator New(int index, int size)
        {
            return new Paginator
            {
                Index = index <= 0 ? 1 : index,
                Size = size <= 0 ? DEFAULT_SIZE : size
            };
        }
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int Size { get; set; } = DEFAULT_SIZE;
        /// <summary>
        /// 每页默认页数
        /// </summary>
        internal const int DEFAULT_SIZE = 20;
        /// <summary>
        /// 总页数
        /// </summary>
        public int TotalPages
        {
            get
            {
                if (TotalRows % Size == 0)
                    return TotalRows / Size;
                return TotalRows / Size + 1;
            }
        }
        /// <summary>
        /// 总数据数
        /// </summary>
        public int TotalRows { get; set; }
        /// <summary>
        /// 参数
        /// </summary>
        [NonSerialized]
        public Dictionary<string, string> Params;
        /// <summary>
        /// 获取跳过的条数
        /// </summary>
        /// <returns></returns>
        public int GetSkip() => Size * (Index - 1);
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public dynamic List { get; set; }
        public List<T> GetList<T>()
        {
            return List;
        }
    }
}
