using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions
{
    public class Hub
    {
        /// <summary>
        /// 获取问题
        /// </summary>
        public async Task<Resp> GetList(Paginator page, Share.Platform platform)
        {
            Resp resp = platform switch
            { 
                Share.Platform.Admin => await GetAdminList(page),
                _ => throw new ArgumentException(),
            };
            return resp;
        }

        private async Task<Resp> GetAdminList(Paginator page)
        {
            using var db = new YGBContext();
            List<Models.QuestionItem_Admin> list = await db.Questions.AsNoTracking()
                                                              .Skip(page.GetSkip())
                                                              .Take(page.Size)
                                                              .OrderByDescending(q => q.CreateDate)
                                                              .Select(q => new Models.QuestionItem_Admin
                                                              {
                                                                  Id = q.Id,
                                                                  Title = q.Title,
                                                                  Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                                                  CreateDate = q.CreateDate.ToStandardString()
                                                              })
                                                              .ToListAsync();
            return Resp.Success(list, "");
        }
    }
}
