using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
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
            string search = pager.Params?["search"] ?? "";

            Expression<Func<DB.Tables.Question, bool>> where = q => q.State == (int)Question.QuestionState.Enabled;
            where = where.And(WhereExpression(search));

            await using var db = new YGBContext();
            
            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Skip(pager.Skip)
                                            .Take(pager.Size)
                                            .OrderByDescending(q => q.CreateDate)
                                            .Where(where)
                                            .Select(q => new Results.QuentionItem_Client
                                            {
                                                Id = q.Id,
                                                Title = q.Title,
                                                Description = q.Description.Length > Question.LIST_DESCRIPTION_LENGTH ? q.Description.Substring(0, Question.LIST_DESCRIPTION_LENGTH) + "..." : q.Description,
                                                CreateDate = q.CreateDate.ToStandardString(),
                                                VoteCounts = q.Votes,
                                                ViewCounts = q.Views,
                                                AnswerCounts = q.Answers.Count(a => a.State == (int)Answers.Answer.AnswerState.Enabled),
                                                Tags = q.Tags.SplitOfChar(','),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
                                            
            return Resp.Success(pager, "");
        }

        /// <summary>
        /// 根据搜索字符拼接查询表达式树
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private Expression<Func<DB.Tables.Question, bool>> WhereExpression(string search)
        {
            string[] searchKeywords = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            Expression<Func<DB.Tables.Question, bool>> where = q => false;
            if (searchKeywords.Length == 0)
                where = q => q.Title.Contains("") || q.Description.Contains("");
            else
            {
                for (int i = 0; i < searchKeywords.Length; i++)
                {
                    string keyword = searchKeywords[i];
                    if (i == 0)
                        where = q => q.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || q.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase);
                    else
                        where = where.Or(q => q.Title.Contains(keyword, StringComparison.OrdinalIgnoreCase) || q.Description.Contains(keyword, StringComparison.OrdinalIgnoreCase));
                }
            }
            return where;
        }
    }
}
