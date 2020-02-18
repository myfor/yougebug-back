using System.Threading.Tasks;

namespace Domain.Questions.List
{
    /// <summary>
    /// 获取问题列表接口
    /// </summary>
    interface IGetQuestionListAsync
    {
        /// <summary>
        /// 获取问题列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        Task<Resp> GetListAsync(Paginator pager);
    }
}
