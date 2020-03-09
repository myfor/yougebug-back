using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Questions
{
    public class Question : BaseEntity
    {
        private const string QUESTION_NO_EXIST = "该问题不存在";

        public enum QuestionState
        {
            All = 0,
            [Description("退回")]
            Back,
            [Description("启用")]
            Enabled,
            [Description("移除")]
            Remove,
            [Description("待审核")]
            ToAudit
        }

        public Question(int id) : base(id)
        {
        }
        /// <summary>
        /// 获取空对象
        /// </summary>
        public static Question GetEmpty() => new Question(EMPTY);

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public string GetTitle()
        {
            CheckEmpty();

            string key = $"c0396d57-7c18-47d5-957c-d135f57882aa_{Id}";
            string name = Cache.Get<string>(key);
            if (name is null)
            {
                using var db = new YGBContext();
                DB.Tables.Question question = db.Questions.AsNoTracking().FirstOrDefault(a => a.Id == Id);
                name = question?.Title ?? "";
                Cache.Set(key, name);
            }
            return name;
        }

        /// <summary>
        /// 举报这个提问
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> ReportAsync(string reason, string description)
        {
            /*
             * 举报这个提问，变成待审核状态
             * 添加一条提问举报记录
             */

            if (string.IsNullOrWhiteSpace(reason))
                return Resp.Fault(Resp.NONE, "请填写举报原因");

            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.State != (int)QuestionState.ToAudit)
                question.State = (int)QuestionState.ToAudit;

            DB.Tables.QuestionReportRecord record = new DB.Tables.QuestionReportRecord
            {
                QuestionId = Id,
                Reason = reason,
                Description = description ?? ""
            };
            db.QuestionReportRecords.Add(record);
            if (await db.SaveChangesAsync() > 0)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "操作失败");
        }

        /// <summary>
        /// 退回一个提问
        /// </summary>
        /// <param name="description">退回理由</param>
        /// <returns></returns>
        public async Task<Resp> BackAsync(string description)
        {
            /*
             * 退回这个提问，该提问变成退回状态
             * 添加一条提问退回记录
             */

            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.State != (int)QuestionState.Back)
                question.State = (int)QuestionState.Back;

            DB.Tables.QuestionBackRecord record = new DB.Tables.QuestionBackRecord
            {
                QuestionId = Id,
                Description = description
            };
            db.QuestionBackRecords.Add(record);
            if (await db.SaveChangesAsync() > 0)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "退回失败");
        }

        /// <summary>
        /// 提交审核
        /// </summary>
        public async Task<Resp> ToAudit(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Resp.Fault(Resp.NONE, "提问内容不能为空");

            using var db = new YGBContext();

            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.Description == description)
                return Resp.Fault(Resp.NONE, "提问内容未修改");
            question.State = (int)QuestionState.ToAudit;
            question.Description = description;
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "提交失败");
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> GetDetailAsync()
        {
            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.AsNoTracking()
                                                            .Include(q => q.Asker)
                                                            .ThenInclude(asker => asker.Avatar)
                                                            .FirstOrDefaultAsync(q => q.Id == Id);

            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);

            //  获取第一页的答案分页
            Paginator page = Paginator.New(1, Paginator.DEFAULT_SIZE);
            (page.List, page.TotalRows) = await GetAnswersAsync(1, Paginator.DEFAULT_SIZE);

            Models.QuestionDetail detail = new Models.QuestionDetail
            {
                Title = question.Title,
                Description = question.Description,
                Tags = question.Tags.Split(','),
                Votes = question.Votes,
                Views = question.Views,
                Actived = question.Actived.ToStandardString(),
                CreateDate = question.CreateDate.ToStandardString(),
                State = Share.KeyValue<int, string>.Create(question.State, question.State.GetDescription<QuestionState>()),
                User = new Clients.Models.UserIntro
                {
                    Id = question.Asker.Id,
                    Account = question.Asker.Name,
                    Avatar = question.Asker.Avatar.Thumbnail
                },
                Page = page
            };
            return Resp.Success(detail, "");
        }

        /// <summary>
        /// 新回答
        /// </summary>
        /// <param name="answererId">回答人ID</param>
        /// <param name="content">回答内容</param>
        /// <returns></returns>
        public async Task<Resp> AddAnswerAsync(int answererId, string content)
        {
            Answers.Hub answerHub = new Answers.Hub();
            (bool isSuccess, string msg) = await answerHub.NewAnswerAsync(Id, content, answererId);
            if (isSuccess)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, msg);
        }

        /// <summary>
        /// 获取问题的答案列表
        /// </summary>
        /// <param name="index">第几页</param>
        /// <param name="size">几条</param>
        /// <returns>返货答案列表和总条数</returns>
        private async Task<(List<Answers.Models.AnswerItem>, int)> GetAnswersAsync(int index, int size)
        {
            using var db = new YGBContext();

            int totalSize = await db.Answers.CountAsync(a => a.QuestionId == Id);
            List<Answers.Models.AnswerItem> list = await db.Answers.AsNoTracking()
                                                                    .Include(a => a.Answerer)
                                                                    .ThenInclude(a => a.Avatar)
                                                                    .Skip((index - 1) * size).Take(size)
                                                                    .OrderByDescending(a => a.Votes)
                                                                    .Select(a => new Answers.Models.AnswerItem
                                                                    {
                                                                        Id = a.Id,
                                                                        Votes = a.Votes,
                                                                        Content = a.Content,
                                                                        CreateDate = a.CreateDate.ToStandardString(),
                                                                        User = a.AnswererId.HasValue ? new Clients.Models.UserIntro
                                                                        {
                                                                            Id = 0,
                                                                            Account = a.NickName,
                                                                            Avatar = File.DEFAULT_AVATAR
                                                                        } : new Clients.Models.UserIntro
                                                                        {
                                                                            Id = a.Id,
                                                                            Account = a.Answerer.Name,
                                                                            Avatar = a.Answerer.Avatar.Thumbnail
                                                                        }
                                                                    })
                                                                    .ToListAsync();
            return (list, totalSize);
        }
    }
}
