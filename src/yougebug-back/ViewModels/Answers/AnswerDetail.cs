using Domain;

namespace yougebug_back.ViewModels.Answers
{
    public class AnswerDetail
    {
        public string QuestionTitle { get; set; } = "";
        public string QuestionContent { get; set; } = "";
        public string AnswerContent { get; set; } = "";
        public Share.KeyValue<int, string> State { get; set; }
        public int AnswererId { get; set; }
        public string AnswererName { get; set; }
        public string AnswererAvatar { get; set; }
        public string CreateDate { get; set; } = "";
        public bool IsSelf { get; set; }
    }
}
