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
        /// 获取问题
        /// </summary>
        public async Task<Resp> GetListAsync(Paginator page, Share.Platform platform)
        {
            Domain.Questions.List.IGetQuestionListAsync questionList = platform switch
            { 
                Share.Platform.Admin => new List.FromAdmin(),
                Share.Platform.Client => new List.FromClient(),
                _ => throw new ArgumentException(),
            };
            return await questionList.GetListAsync(page);
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
        /// 提一个问题
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
                Tags = string.Join(',', questionParams.Tags)
            };

            YGBContext db = new YGBContext();
            db.Add(question);
            if (await db.SaveChangesAsync() != 0)
                return Resp.Success(Resp.NONE, "");

            return Resp.Fault(Resp.NONE, "提交失败");
        }
    }
}
