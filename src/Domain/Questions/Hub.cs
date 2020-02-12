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
    public class Hub
    {
        /// <summary>
        /// 获取问题
        /// </summary>
        public async Task<Resp> GetListAsync(Paginator page, Share.Platform platform)
        {
            Resp resp = platform switch
            { 
                Share.Platform.Admin => await GetAdminListAsync(page),
                _ => throw new ArgumentException(),
            };
            return resp;
        }
        /// <summary>
        /// 获取管理员端的问题列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        private async Task<Resp> GetAdminListAsync(Paginator pager)
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

        /// <summary>
        /// 获取问题对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Question GetQuestion(int id)
        {
            return new Question(id);
        }

        /// <summary>
        /// 提一个问题
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> AskQuestion(Models.PostQuestion questionParams)
        {
            throw new NotImplementedException();
        }
    }
}
