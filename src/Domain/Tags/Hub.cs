using DB;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tags
{
    class Hub
    {
        /// <summary>
        /// 添加标签
        /// </summary>
        public async Task AddTagsAsync(string[] tags)
        {
            /*
             * 添加标签，如果标签已经有了，不会重复添加
             */

            await using YGBContext db = new YGBContext();

            List<DB.Tables.Tag> tagList = new List<DB.Tables.Tag>(tags.Length);
            foreach (string tag in tags)
            {
                if (Tag.IsExistTag(tag))
                    continue;
                tagList.Add(new DB.Tables.Tag
                {
                    Name = tag
                });
            }
            if (tagList.Count > 0)
            {
                db.Tags.AddRange(tagList);
                await db.SaveChangesAsync();
            }
        }
    }
}
