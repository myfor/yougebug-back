namespace yougebug_back.ViewModels.Users
{
    public class UserInfo
    {
        public string UserName { get; set; } = "";
        public string Email { get; set; } = "";
        public string CreateDate { get; set; } = "";
        public string Avatar { get; set; } = "";
        public bool IsSelf { get; set; } = false;
    }
}
