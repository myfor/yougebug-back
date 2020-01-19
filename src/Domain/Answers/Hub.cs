using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Answers
{
    public class Hub
    {
        /// <summary>
        /// 获取分页
        /// </summary>
        public async Task<Resp> GetAnswersAsync(Paginator pager, int questionId)
        {
            using var db = new YGBContext();

            pager.TotalRows = await db.Answers.CountAsync(a => a.QuestionId == questionId);
            pager.List = await db.Answers.AsNoTracking()
                                         .Skip(pager.GetSkip())
                                         .Take(pager.Size)
                                         .OrderByDescending(a => a.Votes)
                                         .Where(a => a.QuestionId == questionId)
                                         .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
