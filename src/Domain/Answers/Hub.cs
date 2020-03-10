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
                                         .Skip(pager.Skip)
                                         .Take(pager.Size)
                                         .OrderByDescending(a => a.Votes)
                                         .Where(whereStatement)
                                         .ToListAsync();
            return Resp.Success(pager, "");
        }

        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="content"></param>
        /// <param name="answererId"></param>
        /// <returns></returns>
        internal async Task<(bool, string)> NewAnswerAsync(int questionId, string content, int answererId)
        {
            if (string.IsNullOrWhiteSpace(content))
                return (false, "回答内容不能为空");

            using var db = new YGBContext();
            DB.Tables.Answer answer = new DB.Tables.Answer
            { 
                QuestionId = questionId,
                Content = content,
                AnswererId = answererId,
                State = (int)Answer.AnswerState.Pass
            };
            db.Answers.Add(answer);
            if (await db.SaveChangesAsync() == 1)
                return (true, "");
            return (false, "回答失败");
        }
        internal async Task<(bool, string)> NewAnswerAsync(int questionId, string content, string nickName)
        {
            //  匿名的要审核

            if (string.IsNullOrWhiteSpace(nickName))
                nickName = "匿名";
            if (string.IsNullOrWhiteSpace(content))
                return (false, "回答内容不能为空");

            using var db = new YGBContext();
            DB.Tables.Answer answer = new DB.Tables.Answer
            {
                QuestionId = questionId,
                Content = content,
                NickName = nickName,
                State = (int)Answer.AnswerState.ToAudit
            };
            db.Answers.Add(answer);
            if (await db.SaveChangesAsync() == 1)
                return (true, "");
            return (false, "回答失败");
        }

        public static Answer GetAnswer(int id)
        {
            return new Answer(id);
        }
    }
}
