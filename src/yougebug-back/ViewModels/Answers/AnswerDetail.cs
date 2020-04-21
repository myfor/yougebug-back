using Domain;

namespace yougebug_back.ViewModels.Answers
{
    public class AnswerDetail
    {
        public int Id { get; set; }
        /// <summary>
        /// 用户信息
        /// </summary>
        public Shared.UserIntro UserIntro { get; set; }
        public string QuestionTitle { get; set; } = "";
        public string QuestionContent { get; set; } = "";
        public string[] Tags { get; set; }
        public string AnswerContent { get; set; } = "";
        public Share.KeyValue<int, string> State { get; set; }
        public int AnswererId { get; set; }
        public string AnswererName { get; set; }
        public string AnswererAvatar { get; set; }
        public int AskerId { get; set; }
        public string AskerName { get; set; }
        public string AskerAvatar { get; set; }
        public string CreateDate { get; set; } = "";
        public bool IsSelf { get; set; }
    }
}
