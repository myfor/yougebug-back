using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 客户端的问题列表
    /// </summary>
    internal class FromClient : IGetQuestionListAsync
    {
        public async Task<Resp> GetListAsync(Paginator pager)
        {
            string search = pager.Params["title"] ?? "";

            Expression<Func<DB.Tables.Question, bool>> where = q =>
                q.Title.Contains(search) || q.Description.Contains(search);

            using var db = new YGBContext();

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Skip(pager.GetSkip())
                                            .Take(pager.Size)
                                            .OrderByDescending(q => q.CreateDate)
                                            .Where(where)
                                            .Select(q => new Models.QuentionItem_Client
                                            {
                                                Id = q.Id,
                                                Title = q.Title,
                                                Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                                CreateDate = q.CreateDate.ToStandardString(),
                                                VoteCounts = q.Votes,
                                                ViewCounts = q.Views,
                                                AnswerCounts = q.Answers.Count(),
                                                Tags = q.Tags.Split(new char[] { ',' }),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
