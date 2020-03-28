using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Administrators
{
    public class Account : BaseEntity
    {
        public Account(int id) : base(id)
        {
        }

        public static Account GetEmpty()
        {
            return new Account(EMPTY);
        }

        /// <summary>
        /// 登录
        /// </summary>
        public static async Task<(bool, Resp)> Login(Models.LoginInfo loginInfo)
        {
            /*
             * 检查登录参数
             */

            (bool isValid, string msg) = loginInfo.IsValid();
            if (!isValid)
                return (false, Resp.Fault(Resp.NONE, msg));

            /*
             * 检查登录账号密码
             */

            using var db = new YGBContext();

            DB.Tables.Admin account = await db.Admins.FirstOrDefaultAsync(a => a.Account == loginInfo.Account && a.Password == loginInfo.Password);

            if (account is null)
                return (false, Resp.Fault(Resp.NONE, "账号不存在或密码错误"));

            account.Token = Guid.NewGuid();
            int suc = await db.SaveChangesAsync();
            if (suc != 1)
                return (false, Resp.Fault(Resp.NONE, "登录失败, 请重试"));

            Results.LoggedInInfo result = new Results.LoggedInInfo
            {
                Id = account.Id,
                Token = account.Token,
                UserName = account.Account,
                Email = account.Email
            };
            return (true, Resp.Success(result));
        }

        /// <summary>
        /// 获取账号
        /// </summary>
        /// <returns></returns>
        public string GetAccount()
        {
            CheckEmpty();

            string key = $"0d376d79-1049-4e0d-8562-896392539b2d_{Id}";
            string name = Cache.Get<string>(key);
            if (name is null)
            {
                using var db = new YGBContext();
                var account = db.Admins.AsNoTracking().FirstOrDefault(a => a.Id == Id);
                name = account?.Account ?? "";
                Cache.Set(key, name);
            }
            return name;
        }

        /// <summary>
        /// 登出
        /// </summary>
        public async Task<Resp> Logout()
        {
            using var db = new YGBContext();
            DB.Tables.Admin account = await db.Admins.FirstOrDefaultAsync(a => a.Id == Id);
            if (account is null)
                return Resp.Success(Resp.NONE);
            account.Token = Guid.NewGuid();
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "退出登录失败");
        }
    }
}
