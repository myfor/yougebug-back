using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 获取管理端的问题列表
    /// </summary>
    internal class FromAdmin : IGetQuestionListAsync
    {
        public async Task<Resp> GetListAsync(Paginator pager)
        {
            string title = pager.Params["title"] ?? "";

            Expression<Func<DB.Tables.Question, bool>> where = q => q.Title.Contains(title);

            using var db = new YGBContext();

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                           .Skip(pager.GetSkip())
                                           .Take(pager.Size)
                                           .OrderByDescending(q => q.CreateDate)
                                           .Where(where)
                                           .Select(q => new Models.QuestionItem_Admin
                                           {
                                               Id = q.Id,
                                               Title = q.Title,
                                               Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                               CreateDate = q.CreateDate.ToStandardString()
                                           })
                                           .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
