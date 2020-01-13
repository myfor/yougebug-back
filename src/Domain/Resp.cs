using System;

namespace Domain
{
    /// <summary>
    /// 响应的结果包装
    /// </summary>
    public class Resp
    {
        /// <summary>
        /// 状态码
        /// </summary>
        public enum Code
        {
            Success,
            Fault,
            NeedLogin,
        }
        /// <summary>
        /// 没有返回数据 data
        /// </summary>
        [NonSerialized]
        public const object NONE = null;
        /// <summary>
        /// 状态码
        /// </summary>
        [NonSerialized]
        public Code StatusCode;
        /// <summary>
        /// 信息
        /// </summary>
        public string Message { get; }
        /// <summary>
        /// 返回数据
        /// </summary>
        public dynamic Data { get; private set; }
        /// <summary>
        /// 获取数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public T GetData<T>() => (T)Convert.ChangeType(Data, typeof(T));
        public void SetData(dynamic data)
        {
            Data = data;
        }

        private Resp(Code code, string msg, dynamic data)
        {
            StatusCode = code;
            Message = msg ?? "";
            Data = data;
        }
        /// <summary>
        /// 成功
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Resp Success(dynamic data = NONE, string msg = "") => new Resp(Code.Success, msg, data);
        /// <summary>
        /// 失败
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Resp Fault(dynamic data = NONE, string msg = "") => new Resp(Code.Fault, msg, data);
        /// <summary>
        /// 需要登录
        /// </summary>
        /// <param name="data"></param>
        /// <param name="msg"></param>
        /// <returns></returns>
        public static Resp NeedLogin(dynamic data = NONE, string msg = "") => new Resp(Code.NeedLogin, msg, data);
    }
}
