using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Answers
{
    public class Hub
    {
        public enum AnswerSource
        {
            /// <summary>
            /// 提问详情
            /// </summary>
            Question,
            /// <summary>
            /// 管理端所有回答列表
            /// </summary>
            AllAnswersOfAdmin,
            /// <summary>
            /// 用户主页的回答列表
            /// </summary>
            UserPage
        }

        /// <summary>
        /// 获取一个用户返回答案列表的对象
        /// </summary>
        internal List.AnswerList GetAnswers(AnswerSource source)
        {
            List.AnswerList answers = source switch
            { 
                AnswerSource.Question => new List.FromQuestion(),
                AnswerSource.AllAnswersOfAdmin => new List.FromAllOfAdmin(),
                AnswerSource.UserPage => new List.FromUserPage(),
                _ => throw new ArgumentException()
            };
            return answers;
        }

        /// <summary>
        /// 获取问题的回答，分页
        /// </summary>
        public async Task<Resp> GetAnswersAsync(Paginator pager, int questionId, Answer.StandardStates answerState = Answer.StandardStates.NoSelected)
        {
            pager["questionId"] = questionId.ToString();
            pager["answerState"] = ((int)answerState).ToString();
            var answers = GetAnswers(AnswerSource.Question);
            var r = await answers.GetListAsync(pager);
            return r;
        }

        /// <summary>
        /// 获取所有答案列表
        /// </summary>
        /// <param name="pager"></param>
        /// <param name="state"></param>
        /// <returns></returns>
        public async Task<Resp> GetAnswersList(Paginator pager, Answer.AnswerState state)
        {
            pager["state"] = ((int)state).ToString();
            var answers = GetAnswers(AnswerSource.AllAnswersOfAdmin);
            var r = await answers.GetListAsync(pager);
            return r;
        }
        
        /// <summary>
        /// クライアントホームページのカイトをしゅ取得する
        /// </summary>
        public async Task<Resp> GetAnswerFormUserPageAsync(Paginator pager)
        {
            var answer = GetAnswers(AnswerSource.UserPage);
            var r = await answer.GetListAsync(pager);
            return r;
        }

        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="questionId"></param>
        /// <param name="content"></param>
        /// <param name="answererId"></param>
        /// <returns></returns>
        internal async Task<(bool, string)> NewAnswerAsync(int questionId, string content, int answererId)
        {
            if (string.IsNullOrWhiteSpace(content))
                return (false, "回答内容不能为空");

            using var db = new YGBContext();
            DB.Tables.Answer answer = new DB.Tables.Answer
            {
                QuestionId = questionId,
                Content = content,
                AnswererId = answererId,
                State = (int)Answer.StandardStates.Enabled
            };
            db.Answers.Add(answer);
            if (await db.SaveChangesAsync() == 1)
                return (true, "");
            return (false, "回答失败");
        }
        internal async Task<(bool, string)> NewAnswerAsync(int questionId, string content, string nickName)
        {
            //  匿名的要审核

            if (string.IsNullOrWhiteSpace(nickName))
                nickName = "匿名";
            if (string.IsNullOrWhiteSpace(content))
                return (false, "回答内容不能为空");

            using var db = new YGBContext();
            DB.Tables.Answer answer = new DB.Tables.Answer
            {
                QuestionId = questionId,
                Content = content,
                NickName = nickName,
                State = (int)Answer.AnswerState.ToAudit
            };
            db.Answers.Add(answer);
            if (await db.SaveChangesAsync() == 1)
                return (true, "");
            return (false, "回答失败");
        }

        public static Answer GetAnswer(int id)
        {
            return new Answer(id);
        }
    }
}
