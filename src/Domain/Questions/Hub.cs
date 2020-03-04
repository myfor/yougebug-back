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
        /// 获取客户端的提问分页列表
        /// </summary>
        /// <param name="page"></param>
        /// <returns></returns>
        public async Task<Paginator> GetClientQuestionPager(Paginator page)
        {
            Resp r = await GetListAsync(page, Share.Platform.Client);
            Paginator pager = r.GetData<Paginator>();

            return pager;
        }

        /// <summary>
        /// 获取最新的提问分页
        /// </summary>
        public async Task<Paginator> GetNewestQuestionsPager(Paginator pager)
        {
            using var db = new YGBContext();

            Expression<Func<DB.Tables.Question, bool>> where = q => q.State == (int)Question.QuestionState.Enabled;

            pager.TotalRows = await db.Questions.CountAsync(where);
            pager.List = await db.Questions.AsNoTracking()
                                            .Include(q => q.Answers)
                                            .Include(q => q.Asker)
                                            .ThenInclude(asker => asker.Avatar)
                                            .Skip(pager.Skip)
                                            .Take(pager.Size)
                                            .OrderByDescending(q => q.CreateDate)
                                            .Where(where)
                                            .Select(q => new Models.QuentionItem_Client
                                            {
                                                Id = q.Id,
                                                Title = q.Title,
                                                Description = q.Description.Length > 20 ? q.Description.Substring(0, 20) + "..." : q.Description,
                                                CreateDate = q.CreateDate.ToStandardString(),
                                                VoteCounts = q.Votes,
                                                ViewCounts = q.Views,
                                                AnswerCounts = q.Answers.Count(),
                                                Tags = q.Tags.SplitOfChar(','),
                                                AskerName = q.Asker.Name,
                                                AskerAvatar = q.Asker.Avatar.Thumbnail
                                            })
                                            .ToListAsync();
            return pager;
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

            if (questionParams.Tags.Length >= 0)
            {
                List<DB.Tables.Tag> tagList = new List<DB.Tables.Tag>(questionParams.Tags.Length);
                foreach (string tag in questionParams.Tags)
                {
                    if (Tags.Tag.IsExistTag(tag))
                        continue;
                    tagList.Add(new DB.Tables.Tag
                    {
                        Name = tag
                    });
                }
                if (tagList.Count > 0)
                    db.Tags.AddRange(tagList);
            }

            db.Add(question);
            if (await db.SaveChangesAsync() != 0)
                return Resp.Success(Resp.NONE, "");

            return Resp.Fault(Resp.NONE, "提交失败");
        }
    }
}
