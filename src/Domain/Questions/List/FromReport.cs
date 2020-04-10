/*
 * 被举报的提问状态不会改变，在审核过后，如果违规，会设置为退回状态
 * 经用户修改后，再经审核即可启用
 */
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 被举报的提问
    /// </summary>
    internal class FromReport : IGetQuestionListAsync
    {
        /// <summary>
        /// 举报列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public Task<Resp> GetListAsync(Paginator pager)
        {
            string title = pager["title"] ?? "";

            throw new NotImplementedException();
        }
    }
}
