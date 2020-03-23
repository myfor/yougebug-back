namespace yougebug_back.Shared
{
    public class Defaults
    {
        /// <summary>
        /// 管理端默认路由前缀
        /// </summary>
        public const string ADMIN_DEFAULT_ROUTE_PREFIX = "admin/api/";
        /// <summary>
        /// 管理端默认路由, admin/api/
        /// </summary>
        public const string ADMIN_DEFAULT_ROUTE = ADMIN_DEFAULT_ROUTE_PREFIX + "[controller]";
        /// <summary>
        /// 管理端登录保存 JWT 的 cookie
        /// </summary>
        public const string ADMIN_AUTH_COOKIE_KEY = "8d0de8f8-61ef-4c56-a23e-de69a5f41681";
        /// <summary>
        /// 客户端默认路由前缀
        /// </summary>
        public const string CLIENT_DEFAULT_ROUTE_PREFIX = "client/api/";
        /// <summary>
        /// 客户端默认路由 client/api/
        /// </summary>
        public const string CLIENT_DEFAULT_ROUTE = CLIENT_DEFAULT_ROUTE_PREFIX + "[controller]";
        /// <summary>
        /// 客户端登录保存 JWT 的 cookie
        /// </summary>
        public const string CLIENT_AUTH_COOKIE_KEY = "cceeb7f6-02ee-41e2-b843-20e33f85d40e";
        /// <summary>
        /// 页面默认的 title
        /// </summary>
        public const string PAGE_DEFAULT_TITLE = "有个bug，程序员的技术问答社区";
    }
}
