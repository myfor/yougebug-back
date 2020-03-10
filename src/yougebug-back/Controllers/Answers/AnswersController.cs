using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace yougebug_back.Controllers.Answers
{
    [Route("[controller]")]
    public class AnswersController : ClientsContorller
    {
        /// <summary>
        /// 同意
        /// </summary>
        [HttpPatch("{id}/like")]
        public async Task<IActionResult> LikeAsync(int id)
        {
            Domain.Answers.Answer answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.LikeAsync();
            return Pack(r);
        }

        /// <summary>
        /// 不同意
        /// </summary>
        [HttpPatch("{id}/unlike")]
        public async Task<IActionResult> UnLikeAsync(int id)
        {
            Domain.Answers.Answer answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.UnLikeAsync();
            return Pack(r);
        }
    }
}
