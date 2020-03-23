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
                                                .FirstOrDefaultAsync(a => (a.Name == loginInfo.Account || a.Email == loginInfo.Account) && a.Password == loginInfo.Password);

            if (user is null)
                return Resp.Fault(Resp.NONE, "账号不存在或密码错误");

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
        /// 获取这个用户的名字
        /// </summary>
        /// <returns></returns>
        public string GetName()
        {
            CheckEmpty();

            string key = $"e2522d10-c5ad-4811-87c3-503efe1a5858_{Id}";
            string name = Cache.Get<string>(key);
            if (name is null)
            {
                using var db = new YGBContext();
                DB.Tables.User account = db.Users.AsNoTracking().FirstOrDefault(a => a.Id == Id);
                name = account?.Name ?? "";
                Cache.Set(key, name);
            }
            return name;
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
            using var db = new YGBContext();

            DB.Tables.User user = await db.Users.AsNoTracking()
                                                .Include(u => u.Avatar)
                                                .FirstOrDefaultAsync(u => u.Id == Id);
            if (user is null)
                return Resp.Fault(Resp.NONE, USER_NOT_EXIST);

            Results.ClientDetail detail = new Results.ClientDetail
            {
                UserName = user.Name,
                Email = user.Email,
                CreateDate = user.CreateDate.ToStandardString(),
                Avatar = user.Avatar.Path,
                State = Share.KeyValue<int, string>.Create(user.State, user.State.GetDescription<UserState>())
            };
            return Resp.Success(detail);
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
