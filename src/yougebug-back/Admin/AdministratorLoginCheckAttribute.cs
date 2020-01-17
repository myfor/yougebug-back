using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using yougebug_back.Auth;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace yougebug_back.Admin
{
    public class AdministratorLoginCheckAttribute : ActionFilterAttribute
    {
        public override Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            string token = JWT.GetJwtInHeader(context.HttpContext.Request.Headers);
            if (string.IsNullOrWhiteSpace(token))
            {
                context.Result = new RedirectResult($"/api/errors/401");
                return base.OnActionExecutionAsync(context, next);
            }
            List<Claim> claimsList = JWT.GetClaims(token).ToList();

            if (!CheckJWTClaims(claimsList) || !CheckAccount(claimsList))
            {
                context.Result = new RedirectResult($"/api/errors/401");
                return base.OnActionExecutionAsync(context, next);
            }

            return base.OnActionExecutionAsync(context, next);
        }

        private bool CheckJWTClaims(List<Claim> claimsList)
        {
            /*
			 * 企业端的声明中要有
			 * pc token
			 * 登陆人员 PId
			 */

            string[] claimsType =
            {
                ClaimTypes.Authentication,
                ClaimTypes.PrimarySid
            };

            foreach (string type in claimsType)
            {
                if (!claimsList.Exists(c => c.Type == type))
                    return false;
            }
            return true;
        }

        private bool CheckAccount(List<Claim> claimsList)
        {
            string token = claimsList.First(c => c.Type == ClaimTypes.Authentication).Value;

            string idStr = claimsList.First(c => c.Type == ClaimTypes.PrimarySid).Value;
            if (!int.TryParse(idStr, out int id))
                return false;

            Domain.Administrators.Account account = Domain.Administrators.Hub.GetAccount(id, token);
            return !account.IsEmpty();
        }
    }
}
