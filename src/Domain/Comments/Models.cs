using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace Domain.Comments
{
    public class Models
    {
        public class NewCommentInfo
        {
            public int PostId { get; set; }
            public string NickName { get; set; }
            public string Content { get; set; }
            public List<IFormFile> Images { get; set; }

            public (bool, string) IsValid()
            {
                if (string.IsNullOrWhiteSpace(NickName))
                    return (false, "Need NickName");
                if (string.IsNullOrWhiteSpace(Content))
                    return (false, "Need Contene");

                return (true, "");
            }
        }
    }
}
