using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace yougebug_back.Filter
{
    /// <summary>
    /// 客户端异常筛选器
    /// </summary>
    public class ClientsExceptionFilter : IExceptionFilter
    {
        public void OnException(ExceptionContext context)
        {
            //  用户不存在
            if (context.Exception.Message == Domain.Clients.User.USER_NOT_EXIST)
            {
                //  显示用户不存在页面
                var result = new ViewResult { ViewName = "NotExistUser" };
                context.Result = result;
            }
        }
    }
}
