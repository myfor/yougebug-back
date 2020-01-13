/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       Cache.cs
 *  Desctiption:    缓存类，其实就是封装了 MemoryCache
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-06-03 11:44:15
 *
 *  Version:        V1.0.0
 ***********************************************/

using Microsoft.Extensions.Caching.Memory;
using System;

namespace Common
{
	/// <summary>
	/// 缓存
	/// </summary>
	public static class Cache
	{
		/// <summary>
		/// 默认缓存实体秒数
		/// </summary>
		private const int DefaultCacheEntrySecond = 3;

        
		static Cache()
		{
			MemoryCacheOptions options = new MemoryCacheOptions();
			MemoryCache = new MemoryCache(options);
			DefaultEntryOptions = new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromSeconds(DefaultCacheEntrySecond));
		}

		/// <summary>
		/// 获取缓存对象
		/// </summary>
		public static IMemoryCache MemoryCache { get; private set; }


		public static MemoryCacheEntryOptions DefaultEntryOptions { get; private set; }

		/// <summary>
		/// 使用默认的 options 存进缓存
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <returns></returns>
		public static T Set<T>(object key, T value) => Set(key, value, DefaultEntryOptions);

		/// <summary>
		/// 使用自定义的 options 存进缓存
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="options"></param>
		/// <returns></returns>
		public static T Set<T>(object key, T value, MemoryCacheEntryOptions options) => MemoryCache.Set(key, value, options);

		/// <summary>
		/// 存进缓存，指定过期时间
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <param name="value"></param>
		/// <param name="second">缓存时间</param>
		/// <returns></returns>
		public static T Set<T>(object key, T value, int second)
		{
			if (second < 0)
				second = 0;

			MemoryCacheEntryOptions option = new MemoryCacheEntryOptions()
				.SetSlidingExpiration(TimeSpan.FromSeconds(second));
			return Set(key, value, option);
		}

		/// <summary>
		/// 根据 key 获取缓存内容
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static object Get(object key) => Get<object>(key);

		/// <summary>
		/// 根据 key 获取缓存内容
		/// </summary>
		/// <typeparam name="T"></typeparam>
		/// <param name="key"></param>
		/// <returns></returns>
		public static T Get<T>(object key) => MemoryCache.Get<T>(key);
        /// <summary>
        /// 移除一个缓存
        /// </summary>
        /// <param name="key"></param>
        public static void Remove(object key) => MemoryCache.Remove(key);
	}
}
