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

            Expression<Func<DB.Tables.Question, bool>> where = q => q.State == (int)Question.QuestionState.Enabled &&
                (q.Title.Contains(search, StringComparison.OrdinalIgnoreCase) || q.Description.Contains(search, StringComparison.OrdinalIgnoreCase));

            //string whereStatement = BuildWhereStatement(search).ToString();

            using var db = new YGBContext();
            /*
            pager.TotalRows = await db.Questions.FromSqlRaw(whereStatement).CountAsync();
            pager.List = await db.Questions.FromSqlRaw(whereStatement)
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Skip(pager.Skip)
                                            .Take(pager.Size)
                                            .OrderByDescending(q => q.CreateDate)
                                            .Select(q => new Models.QuentionItem_Client
                                            {
                                                Id = q.Id,
                                                Title = q.Title,
                                                Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                                CreateDate = q.CreateDate.ToStandardString(),
                                                VoteCounts = q.Votes,
                                                ViewCounts = q.Views,
                                                AnswerCounts = q.Answers.Count(),
                                                Tags = q.Tags.SplitOfChar(','),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
*/
            
            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Skip(pager.Skip)
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
                                                Tags = q.Tags.SplitOfChar(','),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
                                            
            return Resp.Success(pager, "");
        }

        /// <summary>
        /// 根据搜索关键字拼接查询字符串
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        private StringBuilder BuildWhereStatement(string search)
        {
            string[] searchKeyValues = search.Split(' ', StringSplitOptions.RemoveEmptyEntries);

            using var db = new DB.YGBContext();
            DB.Tables.Question question = new DB.Tables.Question();

            StringBuilder where = new StringBuilder(50);
            where.Append($"select * from {nameof(db.Questions)} where {nameof(question.State)} = {(int)Question.QuestionState.Enabled} ");

            if (searchKeyValues.Length > 0)
            {
                where.Append(" and (");
                for (int i = 0; i < searchKeyValues.Length; i++)
                {
                    string keyvalue = searchKeyValues[i].Trim();
                    where.Append($"Title like '%{keyvalue}%' or Description like '%{keyvalue}%'");
                    if (i < searchKeyValues.Length - 1)
                        where.Append("or");
                }
                
                where.Append(")");
            }

            where.Append(";");
            return where;
        }
    }
}
