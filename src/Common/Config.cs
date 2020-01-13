/*  RELEASE NOTE
 *  Copyright (C) 2018 BIRENCHENS
 *  All right reserved
 *
 *  Filename:       Config.cs
 *  Desctiption:    
 *
 *  CreateBy:       BIRENCHENS
 *  CreateDate:     2019-05-31 08:41:54
 *
 *  Version:        V1.0.0
 ***********************************************/

using Microsoft.Extensions.Configuration;
using System;

namespace Common
{
	public static class Config
	{
		public static void SetConfiguration(IConfiguration config)
		{
			Configuration = config;
		}

		/// <summary>
		/// 配置
		/// </summary>
		public static IConfiguration Configuration { get; private set; }
		/// <summary>
		/// 获取配置文件的值
		/// </summary>
		/// <param name="key"></param>
		/// <returns></returns>
		public static string GetValue(string key) => Configuration.GetSection(key).Value;
        /// <summary>
		/// 获取配置文件的值
		/// </summary>
        public static T GetValue<T>(string key)
        {
            string value = GetValue(key);

            T result = (T)Convert.ChangeType(value, typeof(T));
            return result;
        }
		/// <summary>
		/// 获取数据库连接字符串
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public static string GetConnectionString(string name) => Configuration.GetConnectionString(name);
	}
}
