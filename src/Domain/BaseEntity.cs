using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;

namespace Domain
{
    /// <summary>
    /// 基础对象
    /// </summary>
    public abstract class BaseEntity
    {
        /// <summary>
        /// 当前对象的 ID
        /// </summary>
        public int Id { get; }
        /// <summary>
        /// 表示一个空的 ID
        /// </summary>
        public const int EMPTY = -1;

        /// <summary>
        /// 标准状态
        /// </summary>
        public enum StandardStates
        {
            /// <summary>
            /// 无选中状态
            /// </summary>
            [Description("无选中状态")]
            NoSelected = -1,
            /// <summary>
            /// 禁用
            /// </summary>
            [Description("禁用")]
            Disabled = 0,
            /// <summary>
            /// 启用
            /// </summary>
            [Description("启用")]
            Enabled = 1,
            /// <summary>
            /// 移除
            /// </summary>
            [Description("移除")]
            Remove = 2
        }

        /// <summary>
        /// 当前对象实体是否为空
        /// </summary>
        /// <returns></returns>
        public bool IsEmpty() => Id == EMPTY;

        public void CheckEmpty()
        {
            if (IsEmpty())
                throw new Exception("对象为空");
        }

        /// <summary>
        /// 获取主题名字
        /// </summary>
        [MethodImpl(MethodImplOptions.Synchronized)]
        public abstract string GetName();

        /// <summary>
        /// 实例化当前对象实体
        /// </summary>
        /// <param name="id"></param>
        internal BaseEntity(int id)
        {
            Id = id;
        }
    }
}
