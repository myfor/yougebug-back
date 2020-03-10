using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Answers
{
    /// <summary>
    /// 答案
    /// </summary>
    public class Answer : BaseEntity
    {
        public const string NOT_EXIST_ANSWER = "该回答不存在";
        public Answer(int id) : base(id) { }

        /// <summary>
        /// 答案状态
        /// </summary>
        public enum AnswerState
        {
            All = 0,
            [Description("退回")]
            Back,
            /// <summary>
            /// 通过
            /// </summary>
            [Description("通过")]
            Pass,
            /// <summary>
            /// 待审核
            /// </summary>
            [Description("待审核")]
            ToAudit,
        }

        /// <summary>
        /// 举报这个回答
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> ReportAsync(string reason, string description)
        {
            /*
             * 举报这个回答，变成待审核状态
             * 添加一条回答举报记录
             */

            if (string.IsNullOrWhiteSpace(reason))
                return Resp.Fault(Resp.NONE, "请填写举报原因");

            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Answer answer = await db.Answers.FirstOrDefaultAsync(q => q.Id == Id);
            if (answer is null)
                return Resp.Fault(Resp.NONE, NOT_EXIST_ANSWER);
            if (answer.State != (int)AnswerState.ToAudit)
                answer.State = (int)AnswerState.ToAudit;

            DB.Tables.AnswerReportRecord record = new DB.Tables.AnswerReportRecord
            {
                AnswerId = Id,
                Reason = reason,
                Description = description ?? ""
            };
            db.AnswerReportRecords.Add(record);
            if (await db.SaveChangesAsync() > 0)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "操作失败");
        }

        /// <summary>
        /// 退回一个答案
        /// </summary>
        /// <param name="description">退回理由</param>
        /// <returns></returns>
        public async Task<Resp> BackAsync(string description)
        {
            if (string.IsNullOrWhiteSpace(description))
                return Resp.Fault(Resp.NONE, "需要填写退回原因");

            using YGBContext db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.FirstOrDefaultAsync(a => a.Id == Id);
            if (answer is null)
                return Resp.Fault(Resp.NONE, NOT_EXIST_ANSWER);

            if (answer.State == (int)AnswerState.ToAudit || answer.State == (int)AnswerState.Back)
                return Resp.Fault(Resp.NONE, $"已经是{((AnswerState)answer.State).GetDescription()}的状态，不能再次退回");

            DB.Tables.AnswerBackRecord record = new DB.Tables.AnswerBackRecord
            {
                AnswerId = answer.Id,
                Description = description,
            };
            answer.State = (int)AnswerState.ToAudit;
            db.AnswerBackRecords.Add(record);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 2)
                return Resp.Success(Resp.NONE, "退回成功");
            return Resp.Fault(Resp.NONE, "退回失败");
        }

        /// <summary>
        /// 修改完答案后提交去审核
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        public async Task<Resp> ToAudit(string content)
        {
            using YGBContext db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.FirstOrDefaultAsync(a => a.Id == Id);
            if (answer is null)
                return Resp.Fault(Resp.NONE, NOT_EXIST_ANSWER);

            if (answer.Content == content)
                return Resp.Fault(Resp.NONE, "内容未修改");

            answer.Content = content;
            answer.State = (int)AnswerState.ToAudit;
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return Resp.Success(Resp.NONE, "提交成功，待审核");
            return Resp.Success(Resp.NONE, "提交失败，请重试");
        }

        /// <summary>
        /// 同意回答
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> LikeAsync()
        {
            using YGBContext db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.FirstOrDefaultAsync(a => a.Id == Id);
            if (answer is null)
                return Resp.Fault(Resp.NONE, NOT_EXIST_ANSWER);
            answer.Votes++;
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "请求失败");
        }

        /// <summary>
        /// 不同意回答
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> UnLikeAsync()
        {
            using YGBContext db = new YGBContext();

            DB.Tables.Answer answer = await db.Answers.FirstOrDefaultAsync(a => a.Id == Id);
            if (answer is null)
                return Resp.Fault(Resp.NONE, NOT_EXIST_ANSWER);
            answer.Votes--;
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "请求失败");
        }
    }
}
