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
            /// <summary>
            /// 通过
            /// </summary>
            [Description("通过")]
            Pass,
            /// <summary>
            /// 不通过
            /// </summary>
            [Description("不通过")]
            Nopass,
            /// <summary>
            /// 待审核
            /// </summary>
            [Description("待审核")]
            Wait,
            /// <summary>
            /// 待审核
            /// </summary>
            [Description("所有")]
            All
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
                return Resp.Fault();

            if (answer.State == (int)AnswerState.Wait || answer.State == (int)AnswerState.Nopass)
                return Resp.Fault(Resp.NONE, $"已经是{((AnswerState)answer.State).GetDescription()}的状态，不能再次退回");

            DB.Tables.AnswerBackRecord record = new DB.Tables.AnswerBackRecord
            { 
                AnswerId = answer.Id,
                Description = description,
            };
            answer.State = (int)AnswerState.Nopass;
            db.AnswerBackRecords.Add(record);
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 2)
                return Resp.Success(Resp.NONE, "退回成功");
            return Resp.Fault(Resp.NONE, "退回失败");
        }
    }
}
