﻿using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Answers.Detail
{
    /// <summary>
    /// クライアント側の回答
    /// </summary>
    class FromClient : IGetDetail
    {
        public async Task<Resp> GetAnswerDetailAsync(int answerId)
        {
            await using var db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.AsNoTracking()
                                                      .Include(a => a.Question)
                                                      .Include(a => a.Answerer)
                                                      .ThenInclude(user => user.Avatar)
                                                      .FirstOrDefaultAsync();
            if (answer is null)
                return Resp.Fault(Resp.NONE, "答案不存在");

            Results.AnswerDetailForClient detail = new Results.AnswerDetailForClient
            {
                QuestionTitle = answer.Question.Title,
                QuestionContent = answer.Question.Description,
                AnswerContent = answer.Content,
                State = Share.KeyValue<int, string>.Create(answer.State, answer.State.GetDescription<Answer.AnswerState>()),
                CreateDate = answer.CreateDate.ToStandardDateString(),
                User = new Clients.Results.UserIntro
                {
                    Id = answer.Answerer.Id,
                    Account = answer.Answerer.Name,
                    Avatar = answer.Answerer.Avatar.Thumbnail
                }
            };
            return Resp.Success(detail);
        }
    }
}
