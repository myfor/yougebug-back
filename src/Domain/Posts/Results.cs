using System.Collections.Generic;

namespace Domain.Posts
{
    public class Results
    {
        public struct PostItem
        {
            public int Id { get; set; }
            public string NickName { get; set; }
            public string Content { get; set; }
            public string Date { get; set; }
            public Share.Image Img { get; set; }
            public List<Comments.Results.CommentItem> Comments { get; set; }
        }
        
    }
}
