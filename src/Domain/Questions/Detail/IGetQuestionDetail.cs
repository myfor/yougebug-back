using System.Threading.Tasks;

namespace Domain.Questions.Detail
{
    /// <summary>
    /// 获取详情
    /// </summary>
    public interface IGetQuestionDetail
    {
        /// <summary>
        /// 获取详情
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        Task<Resp> GetDetailAsync(int questionId, Paginator pager);
    }
}
