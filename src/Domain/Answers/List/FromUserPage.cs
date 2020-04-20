using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Answers.List
{
    class FromUserPage : AnswerList
    {
        /// <summary>
        /// クライアントホームページの回答リストを取得する
        /// </summary>
        public override async Task<Resp> GetListAsync(Paginator pager)
        {
            /*
             * 
             */

            if (!int.TryParse(pager["userId"], out int userId))
                return Resp.Fault(Resp.NONE, "用户参数有误");
            if (!int.TryParse(pager["currentUserId"], out int currentUserId))
                return Resp.Fault(Resp.NONE, "登录用户有有误");
            string questionTitle = pager["questionTitle"] ?? "";

            Expression<Func<DB.Tables.Answer, bool>> whereStatement = a => a.AnswererId == userId && a.State != (int)Answer.AnswerState.Remove;

            bool isSelf = userId == currentUserId;

            //  
            if (!isSelf)
                whereStatement = whereStatement.And(a => a.State == (int)Answer.AnswerState.Enabled);

            if (!string.IsNullOrWhiteSpace(questionTitle))
                whereStatement = whereStatement.And(a => a.Question.Title.Contains(questionTitle, StringComparison.OrdinalIgnoreCase));

            await using var db = new YGBContext();

            pager.TotalRows = await db.Answers.CountAsync(whereStatement);
            pager.List = await db.Answers.AsNoTracking()
                                         .Where(whereStatement)
                                         .Include(a => a.Question)
                                         .OrderByDescending(a => a.CreateDate)
                                         .Skip(pager.Skip)
                                         .Take(pager.Size)
                                         .Select(a => new Results.AnswerItem_UserPage
                                         {
                                             Id = a.Id,
                                             QuestionId = a.QuestionId,
                                             QuestionTitle = a.Question.Title,
                                             AnswerContent = a.Content.Overflow(50),
                                             State = Share.KeyValue<int, string>.Create(a.State, a.State.GetDescription<Answer.AnswerState>()),
                                             CreateDate = a.CreateDate.ToStandardDateString(),
                                             IsSelf = isSelf
                                         })
                                         .ToListAsync();
            return Resp.Success(pager);
        }
    }
}
