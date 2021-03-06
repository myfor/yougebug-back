﻿using Domain;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace yougebug_back.Controllers.Answers
{
    [Route("[controller]")]
    public class AnswersController : ClientsContorller
    {
        /*
         * 回答列表，在用户主页使用
         */
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetAnswersListAsync(int userId, int index, int size, string questionTitle)
        {
            Paginator pager = Paginator.New(index, size, 2);
            pager["userId"] = userId.ToString();
            pager["currentUserId"] = CurrentUser.Id.ToString();
            pager["questionTitle"] = questionTitle;

            Domain.Answers.Hub answerHub = new Domain.Answers.Hub();

            var r = await answerHub.GetAnswerFormUserPageAsync(pager);
            return Pack(r);
        }

        /// <summary>
        /// 答案详情页
        /// </summary>
        [HttpGet("{id}")]
        public async Task<IActionResult> AnswerDetailPageAsync(int id)
        {
            var answer = Domain.Answers.Hub.GetAnswer(id);
            var r = await answer.GetDetailAsync(Domain.Answers.Answer.DetailSource.Client);
            var data = r.GetData<Domain.Answers.Results.AnswerDetailForClient>();

            ViewModels.Answers.AnswerDetail model = new ViewModels.Answers.AnswerDetail
            {
                Id = data.Id,
                QuestionTitle = data.QuestionTitle,
                QuestionContent = data.QuestionContent,
                Tags = data.Tags,
                AnswerContent = data.AnswerContent,
                State = data.State,
                AnswererId = data.AnswererUser.Id,
                AnswererName = data.AnswererUser.Account,
                AnswererAvatar = data.AnswererUser.Avatar,
                AskerId = data.AskerUser.Id,
                AskerName = data.AskerUser.Account,
                AskerAvatar = data.AskerUser.Avatar,
                CreateDate = data.CreateDate,
                IsSelf = CurrentUser.Id == data.AnswererUser.Id
            };

            SetTitle(data.QuestionTitle);

            return View("AnswerDetail", model);
        }

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
        /// 修改答案
        /// </summary>
        [HttpPut("{id}")]
        public async Task<IActionResult> ModifyAnswerAsync(int id, [FromBody]Share.SingleContent model)
        {
            Domain.Answers.Answer answer = Domain.Answers.Hub.GetAnswer(id);
            var r = await answer.ModifyAsync(model.Content);
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

        /// <summary>
        /// 删除回答
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAsync(int id)
        {
            Domain.Answers.Hub answerHub = new Domain.Answers.Hub();
            var r = await answerHub.DeleteAsync(id);
            return Pack(r);
        }
    }
}
