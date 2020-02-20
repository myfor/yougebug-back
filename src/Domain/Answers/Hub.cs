using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Answers
{
    public class Hub
    {
        /// <summary>
        /// 获取问题的回答，分页
        /// </summary>
        public async Task<Resp> GetAnswersAsync(Paginator pager, int questionId, Answer.AnswerState answerState = Answer.AnswerState.All)
        {
            Expression<Func<DB.Tables.Answer, bool>> whereStatement = a => a.QuestionId == questionId;
            if (answerState != Answer.AnswerState.All)
                whereStatement.And(a => a.State == (int)answerState);

            using var db = new YGBContext();

            pager.TotalRows = await db.Answers.CountAsync(a => a.QuestionId == questionId);
            pager.List = await db.Answers.AsNoTracking()
                                         .Skip(pager.GetSkip())
                                         .Take(pager.Size)
                                         .OrderByDescending(a => a.Votes)
                                         .Where(whereStatement)
                                         .ToListAsync();
            return Resp.Success(pager, "");
        }
    }
}
