using System.Collections.Generic;

namespace Domain.Comments
{
    public class Results
    {
        public struct CommentItem
        {
            public string NickName { get; set; }
            public string Content { get; set; }
            public string Date { get; set; }
            public List<Share.Image> Imgs { get; set; }
        }
    }
}
