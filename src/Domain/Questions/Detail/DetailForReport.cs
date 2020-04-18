using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;

namespace Domain.Questions.Detail
{
    class DetailForReport : IGetQuestionDetail
    {
        /// <summary>
        /// 获取举报的提问详情
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="index"></param>
        /// <param name="size"></param>
        /// <returns></returns>
        public async Task<Resp> GetDetailAsync(int questionId, Paginator page)
        {
            await using var db = new YGBContext();

            DB.Tables.Question question = await db.Questions.AsNoTracking()
                                                            .FirstOrDefaultAsync(q => q.Id == questionId);

            if (question is null)
                return Resp.Fault(Resp.NONE, "该提问不存在");

            Results.ReportQuestionDetail detail = new Results.ReportQuestionDetail
            { 
                Title = question.Title,
                Content = question.Description,
                State = Share.KeyValue<int, string>.Create(question.State, question.State.GetDescription<Question.QuestionState>())
            };
            return Resp.Success(detail);
        }
    }
}
