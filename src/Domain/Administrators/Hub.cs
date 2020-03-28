using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DB;
using Microsoft.EntityFrameworkCore;

namespace Domain.Administrators
{
    public class Hub
    {
        public static Account GetAccount(int id, string token)
        {
            using var db = new YGBContext();

            DB.Tables.Admin account = db.Admins.AsNoTracking()
                                               .FirstOrDefault(a => a.Id == id && a.Token.ToString() == token);
            if (account is null)
                return new Account(Account.EMPTY);
            return new Account(account.Id);
        }
        /// <summary>
        /// 根据 token 获取管理员，如果管理员不存在，会返回管理员空值
        /// </summary>
        /// <param name="token"></param>
        /// <returns></returns>
        public static Account GetAccount(string token)
        {
            using var db = new YGBContext();

            DB.Tables.Admin account = db.Admins.AsNoTracking()
                                               .FirstOrDefault(a => a.Token.ToString() == token);
            if (account is null)
                return new Account(Account.EMPTY);
            return new Account(account.Id);
        }
    }
}
