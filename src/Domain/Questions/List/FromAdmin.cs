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
            string title = pager.Params["search"] ?? "";

            Expression<Func<DB.Tables.Question, bool>> where = q => q.Title.Contains(title, StringComparison.OrdinalIgnoreCase);

            //  获取的列表默认不包括移除的
            if (int.TryParse(pager.Params["state"] ?? "", out int state))
                where.And(q => q.State == state);
            else
                where.And(q => q.State != (int)Question.QuestionState.Remove);

            using var db = new YGBContext();

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                           .Skip(pager.Skip)
                                           .Take(pager.Size)
                                           .OrderByDescending(q => q.CreateDate)
                                           .Where(where)
                                           .Select(q => new Models.QuestionItem_Admin
                                           {
                                               Id = q.Id,
                                               Title = q.Title,
                                               Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                               CreateDate = q.CreateDate.ToStandardString(),
                                               State = Share.KeyValue<int, string>.Create(q.State, q.State.GetDescription<Question.QuestionState>())
                                           })
                                           .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
