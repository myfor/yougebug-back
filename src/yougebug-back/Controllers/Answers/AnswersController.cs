using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace yougebug_back.Controllers.Answers
{
    [Route("[controller]")]
    public class AnswersController : ClientsContorller
    {
        /// <summary>
        /// 追问
        /// </summary>
        [HttpPost("{id}/comment")]
        public async Task<IActionResult> PostCommentAsync(int id, [FromBody]string comment)
        {
            //  必须登录
            if (CurrentUser.IsEmpty())
                return Pack(Domain.Resp.NeedLogin());

            if (string.IsNullOrWhiteSpace(comment))
                return Pack(Domain.Resp.Fault(Domain.Resp.NONE, "追问不能未空"));

            Domain.Answers.Answer answer = Domain.Answers.Hub.GetAnswer(id);
            Domain.Resp r = await answer.AddCommentAsync(CurrentUser.Id, comment);
            return Pack(r);
        }

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
