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
            string title = pager["search"] ?? "";

            Expression<Func<DB.Tables.Question, bool>> where = q => q.Title.Contains(title, StringComparison.OrdinalIgnoreCase);

            //  获取的列表默认不包括移除的
            if (int.TryParse(pager["state"] ?? "", out int state))
                where = where.And(q => q.State == state);
            else
                where = where.And(q => q.State != (int)Question.QuestionState.Remove);

            await using var db = new YGBContext();

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                           .OrderByDescending(q => q.CreateDate)
                                           .Where(where)
                                           .Skip(pager.Skip)
                                           .Take(pager.Size)
                                           .Include(q => q.Answers)
                                           .Select(q => new Results.QuestionItem_Admin
                                           {
                                               Id = q.Id,
                                               Title = q.Title,
                                               Description = q.Description.Length > 10 ? q.Description.Substring(0, 10) + "..." : q.Description,
                                               CreateDate = q.CreateDate.ToStandardString(),
                                               State = Share.KeyValue<int, string>.Create(q.State, q.State.GetDescription<Question.QuestionState>()),
                                               AnswersCount = q.Answers.Count
                                           })
                                           .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
