﻿namespace Domain
{
    public class Paginator
    {
        /// <summary>
        /// 当前页码
        /// </summary>
        public int Index { get; set; }
        /// <summary>
        /// 行数
        /// </summary>
        public int Size { get; set; }
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
        /// 获取跳过的条数
        /// </summary>
        /// <returns></returns>
        public int GetSkip() => Size * (Index - 1);
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public dynamic List { get; set; }
    }
}
