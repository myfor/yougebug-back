using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Answers.Detail
{
    class FromAdmin : IGetDetail
    {
        public async Task<Resp> GetAnswerDetailAsync(int answerId)
        {
            await using var db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.AsNoTracking()
                                                      .Where(a => a.Id == answerId)
                                                      .Include(a => a.Question)
                                                        .ThenInclude(q => q.Asker)
                                                            .ThenInclude(asker => asker.Avatar)
                                                      .Include(a => a.Answerer)
                                                        .ThenInclude(user => user.Avatar)
                                                      .FirstOrDefaultAsync();
            if (answer is null)
                return Resp.Fault(Resp.NONE, "答案不存在");

            Results.AnswerDetailForAdmin detail = new Results.AnswerDetailForAdmin
            {
                Id = answerId,
                QuestionTitle = answer.Question.Title,
                QuestionContent = answer.Question.Description,
                AnswerContent = answer.Content,
                State = Share.KeyValue<int, string>.Create(answer.State, answer.State.GetDescription<Answer.AnswerState>()),
                CreateDate = answer.CreateDate.ToStandardDateString(),
                AnswererUser = new Clients.Results.UserIntro
                {
                    Id = answer.Answerer.Id,
                    Account = answer.Answerer.Name,
                    Avatar = answer.Answerer.Avatar.Thumbnail
                },
                AskerUser = new Clients.Results.UserIntro
                {
                    Id = answer.Question.Asker.Id,
                    Account = answer.Question.Asker.Name,
                    Avatar = answer.Question.Asker.Avatar.Thumbnail
                }
            };
            return Resp.Success(detail);
        }
    }
}
