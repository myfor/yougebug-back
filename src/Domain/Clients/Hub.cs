using DB;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace Domain.Clients
{
    public class Hub
    {
        /// <summary>
        /// 获取用户  
        /// </summary>
        public static User GetUser(int id, string token)
        {
            using var db = new YGBContext();

            DB.Tables.User user = db.Users.AsNoTracking()
                                          .Where(u => u.Id == id && u.Token.ToString() == token)
                                          .FirstOrDefault();
            if (user is null)
                return User.GetEmpty();

            return new User(user.Id);
        }

        /// <summary>
        /// 获取用户
        /// </summary>
        public static User GetUser(int id)
        {
            using var db = new YGBContext();

            DB.Tables.User user = db.Users.AsNoTracking()
                                          .Where(u => u.Id == id)
                                          .FirstOrDefault();
            if (user is null)
                return User.GetEmpty();

            return new User(user.Id);
        }

        /// <summary>
        /// 获取用户  
        /// </summary>
        public static User GetUser(string token)
        {
            using var db = new YGBContext();

            DB.Tables.User user = db.Users.AsNoTracking()
                                          .Where(u => u.Token.ToString() == token)
                                          .FirstOrDefault();
            if (user is null)
                return User.GetEmpty();

            return new User(user.Id);
        }

        /// <summary>
        /// 注册
        /// </summary>
        public async Task<Resp> RegisterAsync(Models.RegisterInfo register)
        {
            (bool isValid, string msg) = register.IsValid();
            if (!isValid)
                return Resp.Fault(Resp.NONE, msg);

            using var db = new YGBContext();
            if (await db.Users.AnyAsync(u => u.Email == register.Email))
                return Resp.Fault(Resp.NONE, "该邮箱已被注册");

            DB.Tables.User newUser = new DB.Tables.User
            {
                Name = Guid.NewGuid().ToString(),
                Email = register.Email,
                Password = register.Password,
                AvatarId = File.DEFAULT_IMG_ID,
                Token = System.Guid.NewGuid(),
                State = (int)User.UserState.Enabled
            };
            db.Users.Add(newUser);
            if (await db.SaveChangesAsync() == 1)
                return Resp.Success(Resp.NONE);
            return Resp.Fault(Resp.NONE, "注册失败，请重试");
        }

        /// <summary>
        /// 获取客户列表
        /// 管理端使用
        /// </summary>
        /// <param name="pager"></param>
        /// <returns></returns>
        public async Task<Resp> GetClientsListAysnc(Paginator pager)
        {
            string search = pager.Params["search"] ?? "";
            using var db = new YGBContext();

            Expression<Func<DB.Tables.User, bool>> whereStatement = u => u.Name.Contains(search) || u.Email.Contains(search);

            pager.TotalRows = await db.Users.CountAsync(whereStatement);

            pager.List = await db.Users.AsNoTracking()
                                       .Skip(pager.Skip)
                                       .Take(pager.Size)
                                       .Where(whereStatement)
                                       .Select(u => new Results.ClientItem
                                       {
                                           Id = u.Id,
                                           UserName = u.Name,
                                           Email = u.Email,
                                           CreateDate = u.CreateDate.ToStandardString()
                                       })
                                       .ToListAsync();
            return Resp.Success(pager);
        }
    }
}
