using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions.Detail
{
    /// <summary>
    /// 管理端的提问详情
    /// </summary>
    public class DetailForAdmin : IGetQuestionDetail
    {
        public async Task<Resp> GetDetailAsync(int questionId, int index, int size)
        {
            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.Include(q => q.Asker)
                                                            .ThenInclude(asker => asker.Avatar)
                                                            .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question is null)
                return Resp.Fault(Resp.NONE, Question.QUESTION_NO_EXIST);

            Answers.Hub answerHub = new Answers.Hub();
            //  获取第一页的答案分页
            Paginator page = Paginator.New(index, size);

            (page.List, page.TotalRows) = await answerHub.GetAnswersAsync(questionId, index, size, Answers.Answer.StandardStates.NoSelected);

            Models.QuestionDetail detail = new Models.QuestionDetail
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
                Page = page
            };

            await db.SaveChangesAsync();

            return Resp.Success(detail, "");
        }
    }
}
