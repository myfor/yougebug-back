using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 首页的问题列表
    /// </summary>
    public class FromHomePage : IGetQuestionListAsync
    {
        public async Task<Resp> GetListAsync(Paginator pager)
        {
            await using var db = new YGBContext();

            Expression<Func<DB.Tables.Question, bool>> where = q => q.State == (int)Question.QuestionState.Enabled;

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                            .OrderByDescending(q => q.CreateDate)
                                            .Where(where)
                                            .Skip(pager.Skip)
                                            .Take(pager.Size)
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Select(q => new Results.QuentionItem_Client
                                            {
                                                Id = q.Id,
                                                Title = q.Title,
                                                Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                                CreateDate = q.CreateDate.ToStandardString(),
                                                VoteCounts = q.Votes,
                                                ViewCounts = q.Views,
                                                AnswerCounts = q.Answers.Count(a => a.State == (int)Answers.Answer.AnswerState.Enabled),
                                                Tags = q.Tags.SplitOfChar(','),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
            return Resp.Success(pager);
        }
    }
}
