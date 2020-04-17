using System.Threading.Tasks;

namespace Domain.Answers.List
{
    class FromQuestion : AnswerList
    {

        public override async Task<Resp> GetListAsync(Paginator pager)
        {
            if (!int.TryParse(pager["questionId"], out int questionId) || !int.TryParse(pager["answerState"], out int answerState))
                return Resp.Fault(Resp.NONE, "参数错误");

            (pager.List, pager.TotalRows) = await GetAnswersAsync(questionId, pager.Index, pager.Size, (Answer.AnswerState)answerState);
            return Resp.Success(pager);
        }
    }
}
