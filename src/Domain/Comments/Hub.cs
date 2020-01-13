using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Comments
{
    public class Hub
    {
        public async Task<Resp> GetListAsync(int postId, int index, int rows)
        {
            Paginator pager = new Paginator
            { 
                Index = index,
                Rows = rows
            };

            using var db = new DB.YGBContext();

            pager.TotalRows = await db.Comments.CountAsync(c => c.PostId == postId);
            pager.List = GetComments(postId, index, rows);

            return Resp.Success(pager);
        }

        /// <summary>
        /// 获取所有评论
        /// </summary>
        public async Task<Resp> GetAllListAsync(int postId)
        {
            using var db = new DB.YGBContext();
            List<Results.CommentItem> list = await db.Comments.AsNoTracking()
                                        .Where(c => c.PostId == postId)
                                        .Select(c => new Results.CommentItem
                                        {
                                            NickName = c.Creator,
                                            Content = c.Content,
                                            Date = c.CreateDate.ToStandardTimeString(),
                                            Imgs = File.GetImagesPath(c.Images.SplitToInt(','))
                                        })
                                        .ToListAsync();
            return Resp.Success(list);
        }

        internal List<Results.CommentItem> GetComments(int postId, int index, int rows)
        {
            using var db = new DB.YGBContext();
            return db.Comments.AsNoTracking()
                                          .Where(c => c.PostId == postId)
                                          .Skip(rows * (index - 1))
                                          .Take(rows)
                                          .Select(c => new Results.CommentItem
                                          {
                                              NickName = c.Creator,
                                              Content = c.Content,
                                              Date = c.CreateDate.ToStandardTimeString(),
                                              Imgs = File.GetImagesPath(c.Images.SplitToInt(','))
                                          })
                                          .ToList();
        }

        public async Task<Resp> NewCommentsAsync(Models.NewCommentInfo info)
        {
            (bool isValid, string msg) = info.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            List<File> files = await File.SaveImagesAsync(info.Images, 100, 100);

            DB.Tables.Comment newComment = new DB.Tables.Comment
            {
                PostId = info.PostId,
                CreateDate = DateTimeOffset.Now,
                Creator = info.NickName,
                Content = info.Content,
                Images = string.Join(", ", files.Select(f => f.Id))
            };

            using var db = new DB.YGBContext();
            db.Comments.Add(newComment);
            int suc = await db.SaveChangesAsync();
            if (suc == 1)
                return Resp.Fault(Resp.NONE, "成功");
            return Resp.Fault(Resp.NONE, "提交失败");
        }
    }
}
