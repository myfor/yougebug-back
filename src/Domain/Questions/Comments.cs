using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Questions
{
    public class Comments
    {
        public const int COMMENT_MAX_LENGTH = 128;

        /// <summary>
        /// 删除一个评论
        /// </summary>
        public async Task<Resp> DeleteAsync(int id)
        {
            await using var db = new YGBContext();

            var comment = await db.QuestionComments.AsNoTracking().FirstOrDefaultAsync(qc => qc.Id == id);
            if (comment != null)
            {
                db.QuestionComments.Remove(comment);
                int changeCount = await db.SaveChangesAsync();
                if (changeCount != 1)
                    return Resp.Fault(Resp.NONE, "删除失败");
            }
            return Resp.Success();
        }
    }
}
