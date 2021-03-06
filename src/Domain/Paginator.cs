﻿using System.Collections.Generic;
using System;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace Domain
{
    public class Paginator
    {
        public static Paginator New(int index, int size, int capacity = 0)
        {
            return new Paginator
            {
                Index = index <= 0 ? 1 : index,
                Size = size <= 0 ? DEFAULT_SIZE : size,
                _params = new Dictionary<string, string>(capacity)
            };
        }
        /// <summary>
        /// 获取或设置参数
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        public string this[string key]
        {
            get => _params[key];
            set => _params[key] = value;
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
        [NonSerialized]
        public const int DEFAULT_SIZE = 20;
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
        //[Obsolete("应该使用 [key]")]
        [NonSerialized]
        private Dictionary<string, string> _params;
        /// <summary>
        /// 获取跳过的条数
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int Skip => Size * (Index - 1);
        /// <summary>
        /// 返回数据列表
        /// </summary>
        public dynamic List { get; set; }
        public List<T> GetList<T>()
        {
            return List;
        }

        /// <summary>
        /// 分页的上一页是否禁用
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string PrePageDisabled => Index <= 1 ? "disabled" : "";

        /// <summary>
        /// 分页的下一页是否禁用
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public string NextPageDisablid => TotalPages <= Index ? "disabled" : "";

        /// <summary>
        /// 开始的页码
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int StartIndex
        {
            get
            {
                const int BASE_NUMBER = 5;

                if (Index <= BASE_NUMBER)
                    return 1;
                if (Index == TotalPages)
                    return TotalPages;

                return Index - BASE_NUMBER;
            }
        }

        /// <summary>
        /// 获取技术的页码
        /// </summary>
        [System.Text.Json.Serialization.JsonIgnore]
        public int EndIndex
        {
            get
            {
                const int BASE_NUMBER = 5;

                if (TotalPages <= 0)
                    return 1;
                if (TotalPages - Index <= BASE_NUMBER)
                    return TotalPages;

                return BASE_NUMBER + StartIndex;
            }
        }

        /// <summary>
        /// 分页的路由缓存
        /// </summary>
        private string routerCache;

        /// <summary>
        /// 获取跳转链接
        /// </summary>
        public string GetLink(HttpRequest request, int index)
        {
            if (routerCache == null)
            {
                StringBuilder router = new StringBuilder(request.Path, 50)
                    .Append("?");

                string indexKey = nameof(Index).ToLower();
                string sizeKey = nameof(Size).ToLower();

                foreach (var item in request.Query)
                {
                    string key = item.Key.ToLower();
                    if (key == indexKey || key == sizeKey)
                        continue;
                    router.Append(key).Append("=").Append(item.Value).Append("&");
                }
                router.Append($"{indexKey}={{0}}&{sizeKey}={{1}}");

                routerCache = router.ToString();
            }
            return string.Format(routerCache, index, Size);
        }

        public string GetPreLink(HttpRequest request)
        {
            if (Index <= 1)
                return "javascript:;";

            return GetLink(request, Index - 1);
        }
        public string GetNextLink(HttpRequest request)
        {
            if (Index >= TotalPages)
                return "javascript:;";

            return GetLink(request, Index + 1);
        }
    }
}
