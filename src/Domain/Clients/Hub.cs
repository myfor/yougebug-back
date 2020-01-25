using DB;
using Microsoft.EntityFrameworkCore;
using System.Linq;

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
    }
}
