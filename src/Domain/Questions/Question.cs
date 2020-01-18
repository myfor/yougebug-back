using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Questions
{
    public class Question : BaseEntity
    {
        private const string QUESTION_NO_EXIST = "该问题不存在";

        public enum QuestionState
        {
            [Description("退回")]
            Back,
            [Description("启用")]
            Enabled,
            [Description("移除")]
            Remove
        }

        public Question(int id) : base(id)
        {
        }
        /// <summary>
        /// 获取空对象
        /// </summary>
        /// <returns></returns>
        public static Question GetEmpty() => new Question(EMPTY);

        /// <summary>
        /// 获取标题
        /// </summary>
        /// <returns></returns>
        public override string GetName()
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
        /// 退回一个提问
        /// </summary>
        public async Task<Resp> BackAsync(int id, string description)
        {
            CheckEmpty();

            using var db = new YGBContext();
            DB.Tables.Question question = await db.Questions.FirstOrDefaultAsync(q => q.Id == id);
            if (question is null)
                return Resp.Fault(Resp.NONE, QUESTION_NO_EXIST);
            if (question.State != (int)QuestionState.Back)
                question.State = (int)QuestionState.Back;

            DB.Tables.BackRecord record = new DB.Tables.BackRecord
            { 
                Description = description
            };
            db.BackRecords.Add(record);
            if (await db.SaveChangesAsync() > 0)
                return Resp.Success(Resp.NONE, "");
            return Resp.Fault(Resp.NONE, "撤回失败");
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
                                                            .FirstOrDefaultAsync(q => q.Id == Id);

        }
    }
}
