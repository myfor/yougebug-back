using System.Threading.Tasks;

namespace Domain.Answers.List
{
    public interface IGetAnswerList
    {
        /// <summary>
        /// 获取列表
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        Task<Resp> GetListAsync(Paginator pager);
    }
}
