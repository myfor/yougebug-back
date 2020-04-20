using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions
{
    /// <summary>
    /// 问题仓库
    /// </summary>
    public class Hub
    {
        /// <summary>
        /// 列表来源
        /// </summary>
        public enum QuestionListSource
        {
            /// <summary>
            /// 管理惯
            /// </summary>
            Admin,
            /// <summary>
            /// 客户端
            /// </summary>
            Client,
            /// <summary>
            /// 客户端用户详情页
            /// </summary>
            ClientUserDetailPage,
            /// <summary>
            /// 首页最新
            /// </summary>
            HomePageNewest,
            /// <summary>
            /// 后台举报列表
            /// </summary>
            Reports
        }

        /// <summary>
        /// 获取问题
        /// </summary>
        public async Task<Resp> GetListAsync(Paginator page, QuestionListSource source)
        {
            Domain.Questions.List.IGetQuestionListAsync questionList = source switch
            {
                QuestionListSource.Admin => new List.FromAdmin(),
                QuestionListSource.Client => new List.FromClient(),
                QuestionListSource.ClientUserDetailPage => new List.FromUserSelf(),
                QuestionListSource.HomePageNewest => new List.FromHomePage(),
                QuestionListSource.Reports => new List.FromReport(),
                _ => throw new ArgumentException(),
            };
            return await questionList.GetListAsync(page);
        }

        /// <summary>
        /// 获取客户端的提问分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<Paginator> GetClientQuestionPager(Paginator page)
        {
            Resp r = await GetListAsync(page, QuestionListSource.Client);
            Paginator pager = r.GetData<Paginator>();

            return pager;
        }

        /// <summary>
        /// 获取最新的提问分页
        /// </summary>
        public async Task<Paginator> GetNewestQuestionsPager(Paginator pager)
        {
            var resp = await GetListAsync(pager, QuestionListSource.HomePageNewest);
            var p = resp.GetData<Paginator>();
            return p;
        }

        /// <summary>
        /// 获取问题对象
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public static Question GetQuestion(int id)
        {
            return new Question(id);
        }

        /// <summary>
        /// 提一个问题，会返回新提问的 ID
        /// </summary>
        public async Task<Resp> AskQuestion(Models.PostQuestion questionParams)
        {
            (bool isValid, string msg) = questionParams.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            DB.Tables.Question question = new DB.Tables.Question
            {
                Title = questionParams.Title,
                Description = questionParams.Description,
                Tags = string.Join(',', questionParams.Tags),
                State = (int)Question.QuestionState.Enabled,
                AskerId = questionParams.UserId
            };

            if (questionParams.Tags.Length >= 0)
            {
                await new Tags.Hub().AddTagsAsync(questionParams.Tags);
            }

            await using YGBContext db = new YGBContext();

            db.Add(question);
            if (await db.SaveChangesAsync() != 0)
                return Resp.Success(question.Id, "");

            return Resp.Fault(Resp.NONE, "提交失败");
        }

        /// <summary>
        /// 删除一个答案
        /// </summary>
        /// <param name="id"></param>
        /// <param name="deep">是否深删除</param>
        /// <returns></returns>
        public async Task<Resp> DeleteQuestionAsync(int id, bool deep = false)
        {
            await using var db = new YGBContext();
            var question = await db.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question is null)
                return Resp.Fault(Resp.NONE, "该问题不存在");

            if (deep)
            {
                List<DB.Tables.Answer> answers = await db.Answers.AsNoTracking().Where(a => a.QuestionId == id).ToListAsync();
                List<int> answersId = answers.Select(a => a.Id).ToList();
                var backRecords = await db.AnswerBackRecords.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();
                var reportRecords = await db.AnswerReportRecords.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();
                var comments = await db.AnswerComments.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();

                db.AnswerBackRecords.RemoveRange(backRecords);
                db.AnswerComments.RemoveRange(comments);
                db.AnswerReportRecords.RemoveRange(reportRecords);
                db.Answers.RemoveRange(answers);

                var backQuestion = await db.QuestionBackRecords.AsNoTracking().Where(q => q.QuestionId == id).ToListAsync();
                var reportQuestion = await db.QuestionReportRecords.AsNoTracking().Where(q => q.QuestionId == id).ToListAsync();
                var commentQuestion = await db.QuestionComments.AsNoTracking().Where(q => q.QuestionId == id).ToListAsync();

                db.QuestionComments.RemoveRange(commentQuestion);
                db.QuestionBackRecords.RemoveRange(backQuestion);
                db.QuestionReportRecords.RemoveRange(reportQuestion);
                db.Questions.Remove(question);

                int changeCount = await db.SaveChangesAsync();
                int count = answers.Count + 1 + backRecords.Count + reportRecords.Count + comments.Count + backQuestion.Count + reportQuestion.Count + commentQuestion.Count;
                if (changeCount == count)
                    return Resp.Success();
            }
            else
            {
                question.State = (int)Question.QuestionState.Remove;
                int changeCount = await db.SaveChangesAsync();
                if (changeCount == 1)
                    return Resp.Success(Resp.NONE);
            }

            return Resp.Fault(Resp.NONE, "删除失败");
        }
    }
}
