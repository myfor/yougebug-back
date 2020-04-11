/*
 * 被举报的提问状态不会改变，在审核过后，如果违规，会设置为退回状态
 * 经用户修改后，变为待审核状态，再管理员经审核即可启用
 */
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 被举报的提问
    /// </summary>
    internal class FromReport : IGetQuestionListAsync
    {
        /// <summary>
        /// 举报提问列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public async Task<Resp> GetListAsync(Paginator pager)
        {
            string title = pager["title"] ?? "";

            await using var db = new YGBContext();
            Expression<Func<DB.Tables.Question, bool>> whereStatement = q => q.Title.Contains(title, StringComparison.OrdinalIgnoreCase) && q.QuestionReportRecords.Count(qr => !qr.IsHandled) != 0;

            pager.TotalRows = await db.Questions.Where(whereStatement).CountAsync();
            pager.List = await db.Questions.AsNoTracking()
                                           .Include(q => q.QuestionReportRecords)
                                           .OrderByDescending(q => q.CreateDate)
                                           .Where(whereStatement)
                                           .Select(q => new Results.ReportQuestionItem 
                                           {
                                               QuestionId = q.Id,
                                               Title = q.Title,
                                               ReportCount = q.QuestionReportRecords.Count,
                                               State = Share.KeyValue<int, string>.Create(q.State, q.State.GetDescription<Question.QuestionState>())
                                           })
                                           .ToListAsync();
            return Resp.Success(pager);
        }
    }
}
