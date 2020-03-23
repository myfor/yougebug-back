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
            (pager.List, pager.TotalRows) = await GetAnswersAsync(questionId, pager.Index, pager.Size, answerState);

            return Resp.Success(pager, "");
        }

        internal async Task<(List<Answers.Models.AnswerItem>, int)> GetAnswersAsync(int questionId, int index, int size, Answer.AnswerState answerState = Answer.AnswerState.All)
        {
            Expression<Func<DB.Tables.Answer, bool>> whereStatement = a => a.QuestionId == questionId;
            if (answerState != Answer.AnswerState.All)
                whereStatement.And(a => a.State == (int)answerState);

            using var db = new YGBContext();

            int totalSize = await db.Answers.CountAsync(a => a.QuestionId == questionId);
            var list = await db.Answers.AsNoTracking()
                                         .Where(whereStatement)
                                         .Skip((index - 1) * size)
                                                                    .Take(size)
                                         .Include(a => a.Answerer)
                                         .ThenInclude(a => a.Avatar)
                                         .OrderByDescending(a => a.Votes)
                                         .Select(a => new Models.AnswerItem
                                         {
                                             Id = a.Id,
                                             Votes = a.Votes,
                                             Content = a.Content,
                                             CreateDate = a.CreateDate.ToStandardString(),
                                             State = Share.KeyValue<int, string>.Create(a.State, a.State.GetDescription<Answers.Answer.AnswerState>()),
                                             User = a.AnswererId.HasValue ? new Clients.Models.UserIntro
                                             {
                                                 Id = a.Id,
                                                 Account = a.Answerer.Name,
                                                 Avatar = a.Answerer.Avatar.Thumbnail
                                             } : new Clients.Models.UserIntro
                                             {
                                                 Id = 0,
                                                 Account = a.NickName,
                                                 Avatar = File.DEFAULT_AVATAR
                                             }
                                         })
                                         .ToListAsync();
            return (list, totalSize);
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
