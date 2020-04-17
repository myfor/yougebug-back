using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Answers.List
{
    class FromAllOfAdmin : AnswerList
    {
        public override async Task<Resp> GetListAsync(Paginator pager)
        {
            if (!int.TryParse(pager["state"], out int state))
                return Resp.Fault(Resp.NONE, "");

            string questionTitle = pager["questionTitle"] ?? "";

            Expression<Func<DB.Tables.Answer, bool>> whereStatement = a => a.State == state;
            if (!string.IsNullOrWhiteSpace(questionTitle))
                whereStatement = whereStatement.And(q => q.Question.Title.Contains(questionTitle));

            await using var db = new YGBContext();

            int totalSize = await db.Answers.CountAsync(whereStatement);
            pager.TotalRows = await db.Answers.CountAsync(whereStatement);
            pager.List = await db.Answers.AsNoTracking()
                                         .Where(whereStatement)
                                         .Skip(pager.Skip)
                                         .Take(pager.Size)
                                         .Include(a => a.Question)
                                         .Include(a => a.Answerer)
                                         .OrderByDescending(a => a.CreateDate)
                                         .Select(a => new Results.AnswerItem_All
                                         {
                                             Id = a.Id,
                                             QuestionTitle = a.Question.Title,
                                             Content = a.Content.Overflow(10, "..."),
                                             Votes = a.Votes,
                                             CreateDate = a.CreateDate.ToStandardDateString(),
                                             AnswererName = a.Answerer.Name,
                                             State = Share.KeyValue<int, string>.Create(a.State, a.State.GetDescription<Answers.Answer.AnswerState>())
                                         })
                                         .ToListAsync();
            return Resp.Success(pager);
        }
    }
}
