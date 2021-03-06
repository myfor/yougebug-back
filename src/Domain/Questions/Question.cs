﻿using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Questions
{
    public class Question : BaseEntity
    {
        public const string QUESTION_NO_EXIST = "该问题不存在";
        /// <summary>
        /// 列表中提问描述简介的长度
        /// </summary>
        public const int LIST_DESCRIPTION_LENGTH = 50;
        public enum QuestionState
        {
            /// <summary>
            /// 退回是退回审核
            /// </summary>
            [Description("禁用")]
            Back = 0,
            [Description("启用")]
            Enabled,
            /*
             * 移除操作只能管理员或用户操作
             * 移除后的提问用户自己也不能看到
             */
            [Description("移除")]
            Remove,
            [Description("待审核")]
            ToAudit,
            /// <summary>
            /// 代表提问被删除，违规的言论会被删除，内容会被屏蔽，不可恢复
            /// </summary>
            [Description("删除")]
            Delete,
            All
        }
        /// <summary>
        /// 提问的详情的源
        /// </summary>
        public enum DetailSource
        {
            Client,
            Admin,
            Report
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
        public async Task<Resp> ReportAsync(string content, string description, int reporterId)
        {
            /*
             * 举报这个提问
             * 添加一条提问举报记录
             */

            if (string.IsNullOrWhiteSpace(content))
                return Resp.Fault(Resp.NONE, "请填写举报原因");

            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);

            DB.Tables.QuestionReportRecord record = new DB.Tables.QuestionReportRecord
            {
                QuestionId = Id,
                Content = content,
                Description = description ?? "",
                CreatorId = reporterId
            };
            db.QuestionReportRecords.Add(record);
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "操作失败");
        }

        /// <summary>
        /// 追问
        /// </summary>
        public async Task<Resp> AddCommentAsyns(int commenterId, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
                return Resp.Fault(Resp.NONE, "追问内容不能为空");
            if (content.Length > Comments.COMMENT_MAX_LENGTH)
                return Resp.Fault(Resp.NONE, $"追问内容不得超过{Comments.COMMENT_MAX_LENGTH}个字");

            await using var db = new YGBContext();

            if (!await db.Questions.AnyAsync(q => q.Id == Id))
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);

            DB.Tables.QuestionComment comment = new DB.Tables.QuestionComment
            { 
                QuestionId = Id,
                CommenterId = commenterId,
                Content = content
            };
            db.QuestionComments.Add(comment);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return Resp.Success();
            return Resp.Fault(Resp.NONE, "追问失败");
        }

        public async Task<Resp> EnabledAsync()
        {
            int enabledValue = (int)QuestionState.Enabled;
            string enabledDescription = QuestionState.Enabled.GetDescription();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.State == enabledValue)
                return Resp.Fault(Resp.NONE, $"已经是{enabledDescription}的状态，不能再次{enabledDescription}");
            question.State = enabledValue;
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "修改失败");
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

            if (string.IsNullOrWhiteSpace(description))
                return Resp.Fault(Resp.NONE, "需要退回理由");

            bool success = await BackQuestionAsync(description);
            if (success)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "退回失败");
        }

        /// <summary>
        /// 回退提问
        /// </summary>
        /// <param name="reason"></param>
        /// <param name="isClearReport">是否清除举报记录</param>
        /// <returns></returns>
        internal async Task<bool> BackQuestionAsync(string reason, bool isClearReport = false)
        {
            await using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                return false;
            if (question.State == (int)QuestionState.Back)
                return false;
            question.State = (int)QuestionState.Back;
            DB.Tables.QuestionBackRecord record = new DB.Tables.QuestionBackRecord
            {
                QuestionId = Id,
                Description = reason
            };
            db.QuestionBackRecords.Add(record);
            int shouldChangeCount = 2;
            if (isClearReport)
            {
                var qrList = await db.QuestionReportRecords.AsNoTracking().Where(qr => qr.QuestionId == Id && !qr.IsHandled).ToListAsync();
                db.QuestionReportRecords.RemoveRange(qrList);
                shouldChangeCount += qrList.Count;
            }

            int changeCount = await db.SaveChangesAsync();
            return changeCount == shouldChangeCount;
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
        /// <param name="source"></param>
        /// <param name="page">答案的分页</param>
        /// <returns></returns>
        public async Task<Resp> GetDetailAsync(DetailSource source, Paginator page)
        {
            CheckEmpty();

            Detail.IGetQuestionDetail detail = source switch
            {
                DetailSource.Admin => new Detail.DetailForAdmin(),
                DetailSource.Client => new Detail.DetailForClient(),
                DetailSource.Report => new Detail.DetailForReport(),
                _ => throw new ArgumentException(),
            };

            return await detail.GetDetailAsync(Id, page);
        }

        /// <summary>
        /// 获取用户编辑的提问信息
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<Results.QuestionEditInfo> GetEditInfoAsync(int questionId)
        {
            await using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.AsNoTracking().FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is null)
                return default;
            return new Results.QuestionEditInfo
            { 
                Id = question.Id,
                Title = question.Title,
                Content = question.Description.Trim(),
                Tags = question.Tags.Split(',')
            };
        }

        /// <summary>
        /// 修改提问
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        public async Task<Resp> EditAsync(Models.EditQuestion model)
        {
            (bool isValid, string msg) = model.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            await using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == Id);
            if (question is null)
                Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.AskerId != model.CurrentUserId)
                return Resp.Fault(Resp.NONE, "不能修改其他人的答案");

            string tags = string.Join(',', model.Tags);

            if (question.Title == model.Title && question.Description == model.Description && question.Tags == tags)
                return Resp.Fault(Resp.NONE, "您还没有进行任何修改");

            question.Title = model.Title;
            question.Description = model.Description;
            question.Tags = tags;
            //  如果不是启用的状态，修改后需要审核
            if (question.State != (int)QuestionState.Enabled)
                question.State = (int)QuestionState.ToAudit;
            int count = await db.SaveChangesAsync();
            if (count != 1)
                return Resp.Fault(Resp.NONE, "修改失败，请重试");
            await new Tags.Hub().AddTagsAsync(model.Tags);
            return Resp.Success();
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

        public async Task<Resp> AddAnswerAsync(string nickName, string content)
        {
            Answers.Hub answerHub = new Answers.Hub();
            (bool isSuccess, string msg) = await answerHub.NewAnswerAsync(Id, content, nickName);
            if (isSuccess)
                return Resp.Success(Resp.NONE, "匿名提交成功");
            return Resp.Fault(Resp.NONE, msg);
        }

        /// <summary>
        /// 同意提问
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> LikeAsync()
        {
            using YGBContext db = new YGBContext();

            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(a => a.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            question.Votes++;
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "请求失败");
        }

        /// <summary>
        /// 不同意提问
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> UnLikeAsync()
        {
            using YGBContext db = new YGBContext();

            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(a => a.Id == Id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            question.Votes--;
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "请求失败");
        }
    }
}
