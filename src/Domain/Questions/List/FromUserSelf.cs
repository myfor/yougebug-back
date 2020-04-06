using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 获取用户自己的提问列表，客户端显示
    /// </summary>
    public class FromUserSelf : IGetQuestionListAsync
    {
        /*
         * 被移除的提问用户自己也不能看到
         * 如果当前获取提问列表的非用户本人
         * 则只能获取到启用的提问
         */

        public async Task<Resp> GetListAsync(Paginator pager)
        {
            Expression<Func<DB.Tables.Question, bool>> whereStatement;

            //  要获取的人的ID
            if (int.TryParse(pager.Params["userId"] ?? "", out int userId))
                whereStatement = q => q.AskerId == userId;
            else
                return Resp.Fault(Resp.NONE, "");

            //  当前登录人ID
            if (!int.TryParse(pager.Params["currentUserId"] ?? "", out int currentUserId))
                currentUserId = 0;
            //  如果不是用户本人，只能看到通过的提问
            bool isSelf = currentUserId == userId;
            if (!isSelf)
                whereStatement = whereStatement.And(q => q.State == (int)Question.QuestionState.Enabled);
            //  如果是，则能看到所有提问

            using var db = new YGBContext();

            pager.TotalRows = await db.Questions.CountAsync(whereStatement);
            pager.List = await db.Questions.AsNoTracking()
                                           .OrderByDescending(q => q.CreateDate)
                                           .Where(whereStatement)
                                           .Skip(pager.Skip)
                                           .Take(pager.Size)
                                           .Include(q => q.Answers)
                                           .Select(q => new Results.QuestionItem_UserSelf
                                           { 
                                               Id = q.Id,
                                               IsSelf = isSelf,
                                               Title = q.Title,
                                               Description = q.Description.Length > Question.LIST_DESCRIPTION_LENGTH ? q.Description.Substring(0, Question.LIST_DESCRIPTION_LENGTH) + "..." : q.Description,
                                               CreateDate = q.CreateDate.ToStandardDateString(),
                                               State = Share.KeyValue<int, string>.Create(q.State, q.State.GetDescription<Question.QuestionState>()),
                                               AnswersCount = q.Answers.Count
                                           })
                                           .ToListAsync();

            return Resp.Success(pager, "");
        }
    }
}
