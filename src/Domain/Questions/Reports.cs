using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions
{
    /// <summary>
    /// 提问的举报
    /// </summary>
    public class Reports
    {
        public async Task<Resp> GetReportsListAsync(Paginator pager)
        {
            if (!int.TryParse(pager["questionId"], out int questionId))
                return Resp.Fault(Resp.NONE, "问题参数有误");

            Expression<Func<DB.Tables.QuestionReportRecord, bool>> whereStatement = q => q.QuestionId == questionId && !q.IsHandled;

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
                                                           Content = q.Content,
                                                           Description = q.Description
                                                       })
                                                       .ToListAsync();
            return Resp.Success(pager); ;
        }
    }
}
