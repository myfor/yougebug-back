namespace yougebug_back.ViewModels.Users
{
    /// <summary>
    /// 用户问题列表
    /// </summary>
    public class UserQuestions
    {
        public Shared.UserIntro UserIntro { get; set; }
        public Domain.Paginator Paginator { get; set; }
    }
}
