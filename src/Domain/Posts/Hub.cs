using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Domain.Posts
{
    public class Hub
    {

        public async Task<Resp> GetLiseAsync(int index, int rows)
        {
            Paginator pager = new Paginator
            {
                Index = index,
                Rows = rows
            };

            Comments.Hub commentHub = new Comments.Hub();

            using var db = new DB.YGBContext();
            pager.TotalRows = await db.Posts.CountAsync();
            pager.List = await db.Posts.AsNoTracking()
                                       .OrderByDescending(p => p.CreateDate)
                                       .Skip(pager.GetSkip())
                                       .Take(pager.Rows)
                                       .Include(p => p.Image)
                                       .Select(p => new Results.PostItem
                                       {
                                           Id = p.Id,
                                           NickName = p.Creator,
                                           Content = p.Content,
                                           Date = p.CreateDate.ToStandardTimeString(),
                                           Comments = commentHub.GetComments(p.Id, 1, 5),
                                           Img = new Share.Image
                                           {
                                               Thumbnail = p.Image.Thumbnail,
                                               Source = p.Image.Path
                                           }
                                       })
                                       .ToListAsync();
            return Resp.Success(pager, "");
        }

        public async Task<Resp> NewPostsAsync(Models.NewPostInfo info)
        {
            (bool isValid, string msg) = info.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            File files = await File.SaveImageAsync(info.Img);

            DB.Tables.Post newPost = new DB.Tables.Post
            {
                CreateDate = DateTimeOffset.Now,
                Creator = info.NickName,
                Content = info.Content,
                ImageId = files?.Id ?? File.DEFAULT_IMG_ID
            };
            using var db = new DB.YGBContext();
            db.Posts.Add(newPost);
            int suc = await db.SaveChangesAsync();
            if (suc == 1)
                return Resp.Success(Resp.NONE, "成功");
            return Resp.Fault(Resp.NONE, "提交失败");
        }
    }
}
