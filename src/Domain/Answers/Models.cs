namespace Domain.Answers
{
    public class Models
    {
        /// <summary>
        /// 新的答案
        /// </summary>
        public class PostAnswer
        {
            public string Content { get; set; }
            public string NickName { get; set; }
            public bool IsLogin { get; set; }
        }
    }
}
