using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Administrators
{
    public class Account : BaseEntity
    {
        public Account(int id) : base(id)
        {
        }

        /// <summary>
        /// 登录
        /// </summary>
        public static async Task<Resp> Login(Models.LoginInfo loginInfo)
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

            DB.Tables.Admin account = await db.Admins.FirstOrDefaultAsync(a => a.Account == loginInfo.Account && a.Password == loginInfo.Password);

            if (account is null)
                return Resp.Fault(Resp.NONE, "账号不存在或密码错误");

            account.Token = Guid.NewGuid();
            int suc = await db.SaveChangesAsync();
            if (suc != 1)
                return Resp.Fault(Resp.NONE, "登录失败, 请重试");

            Results.LoggedInInfo result = new Results.LoggedInInfo
            {
                Id = account.Id,
                Token = account.Token,
                UserName = account.Account,
                Email = account.Email
            };
            return Resp.Success(result);
        }

        public override string GetName()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// 登出
        /// </summary>
        public async Task<Resp> Logout()
        {
            using var db = new YGBContext();
            DB.Tables.Admin account = await db.Admins.AsNoTracking().FirstOrDefaultAsync(a => a.Id == Id);
            throw new NotImplementedException();
        }
    }
}
