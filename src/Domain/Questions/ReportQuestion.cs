using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Questions
{
    /// <summary>
    /// 被举报的提问
    /// </summary>
    public class ReportQuestion
    {
        private readonly int _id;

        public ReportQuestion(int id)
        {
            _id = id;
        }


        /// <summary>
        /// 获取该提问的举报列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public async Task<Resp> GetReportsListAsync(Paginator pager)
        {

            Expression<Func<DB.Tables.QuestionReportRecord, bool>> whereStatement = q => q.QuestionId == _id && !q.IsHandled;

            await using var db = new YGBContext();
            pager.TotalRows = await db.QuestionReportRecords.CountAsync(whereStatement);
            pager.List = await db.QuestionReportRecords.AsNoTracking()
                                                       .Where(whereStatement)
                                                       .OrderByDescending(q => q.CreateDate)
                                                       .Skip(pager.Skip)
                                                       .Take(pager.Size)
                                                       .Select(q => new Results.ReportItem
                                                       {
                                                           Id = q.Id,
                                                           Content = q.Description,
                                                           Reason = q.Content
                                                       })
                                                       .ToListAsync();
            return Resp.Success(pager); ;
        }

        /// <summary>
        /// 忽略举报
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> IgnoreAsync()
        {
            /*
             * 清除所有举报记录
             * 不改变提问状态
             */

            await using var db = new YGBContext();

            List<DB.Tables.QuestionReportRecord> list = await db.QuestionReportRecords.AsNoTracking()
                                                                                      .Where(qr => qr.QuestionId == _id)
                                                                                      .ToListAsync();

            if (list.Count == 0)
                return Resp.Success();

            list.ForEach(qr => qr.IsHandled = true);
            db.QuestionReportRecords.UpdateRange(list);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == list.Count)
                return Resp.Success();
            return Resp.Fault(Resp.NONE, "忽略失败");
        }

        /// <summary>
        /// 退回提问
        /// </summary>
        /// <param name="reason"></param>
        /// <returns></returns>
        public async Task<Resp> BackAsync(string reason)
        {
            if (string.IsNullOrWhiteSpace(reason))
                return Resp.Fault(Resp.NONE, "需要退回理由");

            var question = Hub.GetQuestion(_id);
            bool success = await question.BackQuestionAsync(reason, true);
            if (success)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "退回失败");
        }

        /// <summary>
        /// 彻底删除
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> DeleteAsync()
        {
            Reports questionHub = new Reports();
            return await questionHub.DeleteAsync(_id);
        }
    }
}
