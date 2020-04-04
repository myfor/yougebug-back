using System.Collections.Generic;

namespace yougebug_back.ViewModels.Users
{
    public class UserInfo
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string CreateDate { get; set; } = "";
        public string Avatar { get; set; } = "";
        public bool IsSelf { get; set; } = false;
        /// <summary>
        /// 该用户自己的提问
        /// </summary>
        public List<Domain.Questions.Results.QuestionItem_UserSelf> UserAsks { get; set; }
    }
}
