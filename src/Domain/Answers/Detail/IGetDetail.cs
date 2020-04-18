using System.Threading.Tasks;

namespace Domain.Answers.Detail
{
    interface IGetDetail
    {
        /// <summary>
        /// 获取答案详情
        /// </summary>
        Task<Resp> GetAnswerDetailAsync(int answerId);
    }
}
