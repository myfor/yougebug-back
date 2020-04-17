using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Answers.List
{
    abstract class AnswerList : IGetAnswerList
    {
        /// <summary>
        /// 获取答案列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public abstract Task<Resp> GetListAsync(Paginator pager);

        /// <summary>
        /// 获取答案列表分页
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <param name="answerState"></param>
        /// <returns></returns>
        internal async Task<(List<Answers.Results.AnswerItem>, int)> GetAnswersAsync(int questionId, int index, int size, Answer.AnswerState answerState = Answer.AnswerState.NoSelected)
        {
            Expression<Func<DB.Tables.Answer, bool>> whereStatement = a => a.QuestionId == questionId;
            if (answerState != Answer.AnswerState.NoSelected)
                whereStatement = whereStatement.And(a => a.State == (int)answerState);

            await using var db = new YGBContext();

            int totalSize = await db.Answers.CountAsync(whereStatement);
            var list = await db.Answers.AsNoTracking()
                                         .Where(whereStatement)
                                         .OrderByDescending(a => a.Votes)
                                         .Skip((index - 1) * size)
                                         .Take(size)
                                         .Include(a => a.Answerer)
                                         .ThenInclude(a => a.Avatar)
                                         .Select(a => new Results.AnswerItem
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
    }
}
