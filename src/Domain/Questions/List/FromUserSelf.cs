using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 获取用户自己的提问列表，客户端显示
    /// </summary>
    public class FromUserSelf : IGetQuestionListAsync
    {
        /*
         * 如果当前获取提问列表的非用户本人
         * 则只能获取到启用的提问
         */

        public Task<Resp> GetListAsync(Paginator pager)
        {
            throw new NotImplementedException();
        }
    }
}
