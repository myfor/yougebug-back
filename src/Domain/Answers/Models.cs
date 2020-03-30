namespace Domain.Answers
{
    public class Models
    {
        /// <summary>
        /// 答案单项
        /// </summary>
        public struct AnswerItem
        {
            public int Id { get; set; }
            public string Content { get; set; }
            public int Votes { get; set; }
            public string CreateDate { get; set; }
            public Clients.Models.UserIntro User { get; set; }
            public Share.KeyValue<int, string> State { get; set; }
        }
    }
}
