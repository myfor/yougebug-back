using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Clients
{
    /// <summary>
    /// 代表一个用户对象
    /// </summary>
    public class User : BaseEntity
    {
        public enum UserState
        {
            [Description("禁用")]
            Disabled,
            [Description("启用")]
            Enabled
        }

        public const string USER_NOT_EXIST = "用户不存在";
        public User(int id) : base(id)
        {
        }

        /// <summary>
        /// 获取一个空用户
        /// </summary>
        /// <returns></returns>
        public static User GetEmpty() => new User(EMPTY);

        /// <summary>
        /// 登录
        /// </summary>
        public static async Task<Resp> LoginAsync(Models.LoginInfo loginInfo)
        {
            /*
             * 检查登录参数
             */

            (bool isValid, string msg) = loginInfo.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            /*
             * 检查登录账号密码
             */

            using var db = new YGBContext();

            DB.Tables.User user = await db.Users.Include(u => u.Avatar)
                                                .FirstOrDefaultAsync(a =>
                                                (a.Name.Equals(loginInfo.Account, StringComparison.OrdinalIgnoreCase) || a.Email.Equals(loginInfo.Account, StringComparison.OrdinalIgnoreCase))
                                                && a.Password == loginInfo.Password);

            if (user is null)
                return Resp.Fault(Resp.NONE, "用户名或邮箱不存在或密码错误");

            user.Token = Guid.NewGuid();
            int suc = await db.SaveChangesAsync();
            if (suc != 1)
                return Resp.Fault(Resp.NONE, "登录失败, 请重试");

            Results.LoggedInInfo result = new Results.LoggedInInfo
            {
                Avatar = user.Avatar.Thumbnail,
                Id = user.Id,
                Token = user.Token,
                Name = user.Name
            };
            return Resp.Success(result);
        }

        /// <summary>
        /// 登出
        /// </summary>
        public async Task<Resp> LogoutAsync()
        {
            using var db = new YGBContext();
            DB.Tables.User account = await db.Users.FirstOrDefaultAsync(a => a.Id == Id);
            if (account is null)
                return Resp.Success(Resp.NONE);
            account.Token = Guid.NewGuid();
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "退出登录失败");
        }

        /// <summary>
        /// 用户名字缓存
        /// </summary>
        private string _name;
        /// <summary>
        /// 获取这个用户的名字
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            CheckEmpty();

            if (_name != null)
                return _name;

            string key = $"e2522d10-c5ad-4811-87c3-503efe1a5858_{Id}";
            _name = Cache.Get<string>(key);
            if (_name is null)
            {
                using var db = new YGBContext();
                DB.Tables.User account = db.Users.AsNoTracking()
                                                 .Include(u => u.Avatar)
                                                 .FirstOrDefault(a => a.Id == Id);
                _name = account?.Name ?? "";
                _avatar = account.Avatar.Thumbnail;
                Cache.Set(key, _name);
            }
            return _name;
        }
        private string _avatar;
        /// <summary>
        /// 获取用户头像
        /// </summary>
        /// <returns></returns>
        public string GetAvatar()
        {
            CheckEmpty();

            if (_avatar != null)
                return _avatar;

            string key = $"06ba9d74-dd08-4c0f-acf7-cbdcbb56bf40_{Id}";
            _avatar = Cache.Get<string>(key);
            if (_avatar is null)
            {
                using var db = new YGBContext();
                DB.Tables.User account = db.Users.AsNoTracking()
                                                 .Include(u => u.Avatar)
                                                 .FirstOrDefault(a => a.Id == Id);
                _name = account?.Name ?? "";
                _avatar = account.Avatar.Thumbnail;
                Cache.Set(key, _avatar);
            }
            return _avatar;
        }

        /// <summary>
        /// 修改用户名
        /// </summary>
        public async Task<Resp> ChangeUserInfoAsync(Models.UserModify model)
        {
            (bool isValid, string msg) = model.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            using var db = new YGBContext();
            DB.Tables.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null)
                return Resp.Fault(Resp.NONE, USER_NOT_EXIST);

            if (user.Name.Equals(model.UserName, StringComparison.OrdinalIgnoreCase))
                return Resp.Fault(Resp.NONE, "不能和原来的用户相同");
            if (await db.Users.AnyAsync(u => u.Name.Equals(model.UserName, StringComparison.OrdinalIgnoreCase) && u.Id != Id))
                return Resp.Fault(Resp.NONE, "已经被使用的用户名");

            if (await db.Users.AnyAsync(u => u.Email.Equals(model.Email, StringComparison.OrdinalIgnoreCase) && u.Id != Id))
                return Resp.Fault(Resp.NONE, "已经被使用的邮箱");

            user.Name = model.UserName;
            user.Email = model.Email;
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "修改失败");
        }

        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="newPassword">经过加密的新密码</param>
        /// <returns></returns>
        public async Task<Resp> ChangePasswordAsync(Models.ChangePassword model)
        {
            (bool isValid, string msg) = model.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            using var db = new YGBContext();
            DB.Tables.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null)
                return Resp.NeedLogin(Resp.NONE, "请重新登录");
            if (!user.Password.Equals(model.OldPassword, StringComparison.OrdinalIgnoreCase))
                return Resp.Fault(Resp.NONE, "旧密码有误");
            if (user.Password.Equals(model.NewPassword, StringComparison.OrdinalIgnoreCase))
                return Resp.Fault(Resp.NONE, "新密码不能和旧密码相同");
            user.Password = model.NewPassword;
            int changeCount = await db.SaveChangesAsync();
            if (changeCount == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "修改失败");
        }

        /// <summary>
        /// 提问
        /// </summary>
        public async Task<Resp> AskQuestion(Questions.Models.PostQuestion questionParams)
        {
            Questions.Hub hub = new Questions.Hub();
            return await hub.AskQuestion(questionParams);
        }

        /// <summary>
        /// 获取详情
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> GetDetailAsync()
        {
            try
            {
                var detail = await GetUserInfoAsync();
                return Resp.Success(detail);
            }
            catch (Exception ex)
            {
                return Resp.Fault(Resp.NONE, ex.Message);
            }
        }

        public async Task<Results.ClientDetail> GetUserInfoAsync()
        {
            using var db = new YGBContext();

            DB.Tables.User user = await db.Users.AsNoTracking()
                                                .Include(u => u.Avatar)
                                                .FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null)
                throw new NullReferenceException(USER_NOT_EXIST);

            Results.ClientDetail detail = new Results.ClientDetail
            {
                UserName = user.Name,
                Email = user.Email,
                CreateDate = user.CreateDate.ToStandardDateString(),
                Avatar = user.Avatar.Path,
                State = Share.KeyValue<int, string>.Create(user.State, user.State.GetDescription<UserState>())
            };
            return detail;
        }

        /// <summary>
        /// 获取自己的提问
        /// </summary>
        /// <returns></returns>
        public async Task<Paginator> GetSelfQuestionsAsync(Domain.Paginator pager)
        {
            Domain.Questions.Hub questionsHub = new Questions.Hub();
            Resp resp = await questionsHub.GetListAsync(pager, Questions.Hub.QuestionListSource.ClientUserDetailPage);
            Paginator resultPager = resp.GetData<Paginator>();

            return resultPager;
        }
        /// <summary>
        /// 获取用户自己的提问，只获取第一页
        /// </summary>
        public async Task<List<Questions.Models.QuestionItem_UserSelf>> GetSelfQuestionsByDetailAsync(int currentUserId)
        {
            /*
             * currentUserId 为当前查看人的ID
             * userName 为获取的用户名
             */

            Paginator pager = Paginator.New(1, 4);
            pager.Params = new Dictionary<string, string>
            {
                ["userId"] = Id.ToString(),
                ["currentUserId"] = currentUserId.ToString()
            };

            var resultPager = await GetSelfQuestionsAsync(pager);
            List<Questions.Models.QuestionItem_UserSelf> list = resultPager.GetList<Questions.Models.QuestionItem_UserSelf>();
            return list;
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> EnabledAsync()
        {
            string msg = await ModifyState(UserState.Enabled);
            if (string.IsNullOrWhiteSpace(msg))
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, msg);
        }

        public async Task<Resp> DisabledAsync()
        {
            string msg = await ModifyState(UserState.Disabled);
            if (string.IsNullOrWhiteSpace(msg))
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, msg);
        }

        private async Task<string> ModifyState(UserState userState)
        {
            using var db = new YGBContext();

            DB.Tables.User user = await db.Users.FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null)
                return USER_NOT_EXIST;

            int value = (int)userState;
            string description = userState.GetDescription();
            if (user.State == value)
                return $"用户已是{description}的状态，不能重复{description}";
            user.State = value;

            int count = await db.SaveChangesAsync();
            if (count == 1)
                return "";
            return "启用失败";
        }
    }
}
