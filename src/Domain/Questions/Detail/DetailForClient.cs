using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.Detail
{
    /// <summary>
    /// 客户端的问题详情
    /// </summary>
    public class DetailForClient : IGetQuestionDetail
    {
        public async Task<Resp> GetDetailAsync(int questionId, int index, int size)
        {
            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.Include(q => q.Asker)
                                                            .ThenInclude(asker => asker.Avatar)
                                                            .Include(q => q.QuestionComments)
                                                            .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question is null)
                return Resp.Fault(Resp.NONE, Question.QUESTION_NO_EXIST);

            if (question.State != (int)Question.QuestionState.Enabled)
                return Resp.Fault(Resp.NONE, "该提问暂时无法查看");

            Answers.Hub answerHub = new Answers.Hub();
            //  获取第一页的答案分页
            Paginator page = Paginator.New(index, size);
            //(page.List, page.TotalRows) = await GetAnswersAsync(index, size);
            (page.List, page.TotalRows) = await answerHub.GetAnswersAsync(questionId, index, size, Answers.Answer.StandardStates.Enabled);

            Results.QuestionDetailForClient detail = new Results.QuestionDetailForClient
            {
                Id = question.Id,
                Title = question.Title,
                Description = question.Description,
                Tags = question.Tags.Split(','),
                Votes = question.Votes,
                Views = question.Views,
                Actived = question.Actived.ToStandardString(),
                CreateDate = question.CreateDate.ToStandardString(),
                State = Share.KeyValue<int, string>.Create(question.State, question.State.GetDescription<Question.QuestionState>()),
                User = new Clients.Models.UserIntro
                {
                    Id = question.Asker.Id,
                    Account = question.Asker.Name,
                    Avatar = question.Asker.Avatar.Thumbnail
                },
                Page = page,
                Comments = question.QuestionComments.Take(5).Select(c => c.Content).ToArray()
            };

            question.Views++;
            question.Actived = DateTimeOffset.Now;
            await db.SaveChangesAsync();

            return Resp.Success(detail, "");
        }
    }
}
