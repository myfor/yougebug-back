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
    /// 提问的举报
    /// </summary>
    public class Reports
    {
        public static ReportQuestion GetReportQuestion(int questionId)
        {
            return new ReportQuestion(questionId);
        }

        /// <summary>
        /// 删除举报的提问
        /// </summary>
        /// <param name="questionId"></param>
        /// <returns></returns>
        public async Task<Resp> DeleteAsync(int questionId)
        {
            /*
             * 将提问标记为删除
             * 删除相关的其他记录
             * 不可恢复
             */

            await using var db = new YGBContext();
            var question = await db.Questions.FirstOrDefaultAsync(q => q.Id == questionId);
            if (question is null)
                return Resp.Fault(Resp.NONE, "该问题不存在");

            List<DB.Tables.Answer> answers = await db.Answers.AsNoTracking().Where(a => a.QuestionId == questionId).ToListAsync();
            List<int> answersId = answers.Select(a => a.Id).ToList();
            var backRecords = await db.AnswerBackRecords.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();
            var reportRecords = await db.AnswerReportRecords.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();
            var comments = await db.AnswerComments.AsNoTracking().Where(a => answersId.Contains(a.AnswerId)).ToListAsync();

            db.AnswerBackRecords.RemoveRange(backRecords);
            db.AnswerComments.RemoveRange(comments);
            db.AnswerReportRecords.RemoveRange(reportRecords);
            db.Answers.RemoveRange(answers);

            var backQuestion = await db.QuestionBackRecords.AsNoTracking().Where(q => q.QuestionId == questionId).ToListAsync();
            var reportQuestion = await db.QuestionReportRecords.AsNoTracking().Where(q => q.QuestionId == questionId).ToListAsync();
            var commentQuestion = await db.QuestionComments.AsNoTracking().Where(q => q.QuestionId == questionId).ToListAsync();

            db.QuestionComments.RemoveRange(commentQuestion);
            db.QuestionBackRecords.RemoveRange(backQuestion);
            db.QuestionReportRecords.RemoveRange(reportQuestion);

            question.Title = "该提问因违反法律法规已被删除";
            question.Description = "该提问因违反法律法规已被删除，请对自己的言论负责";
            question.State = (int)Question.QuestionState.Delete;

            int changeCount = await db.SaveChangesAsync();
            if (changeCount == answers.Count + 1 + backRecords.Count + reportRecords.Count + comments.Count + backQuestion.Count + reportQuestion.Count + commentQuestion.Count)
                return Resp.Success();

            return Resp.Fault(Resp.NONE, "删除失败");
        }
    }
}
