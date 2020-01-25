using Common;
using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Clients
{
    public class User : BaseEntity
    {
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

            DB.Tables.User user = await db.Users.FirstOrDefaultAsync(a => (a.Name == loginInfo.Account || a.Email == loginInfo.Account) && a.Password == loginInfo.Password);

            if (user is null)
                return Resp.Fault(Resp.NONE, "账号不存在或密码错误");

            user.Token = Guid.NewGuid();
            int suc = await db.SaveChangesAsync();
            if (suc != 1)
                return Resp.Fault(Resp.NONE, "登录失败, 请重试");

            Results.LoggedInInfo result = new Results.LoggedInInfo
            {
                Name = user.Name
            };
            return Resp.Success(result);
        }

        /// <summary>
        /// 登出
        /// </summary>
        /// <returns></returns>
        public async Task<Resp> Logout()
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

        public override string GetName()
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
    }
}
