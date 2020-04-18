using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

namespace yougebug_back.Admin.Clients
{
    [Route(Defaults.ADMIN_DEFAULT_ROUTE)]
    public class ClientsController : AdminBaseController
    {
        
        /*
        获取列表
         */
        [HttpGet]
        public async Task<IActionResult> GetListAsync(int index, int size, string search)
        {
            Paginator pager = Paginator.New(index, size);
            pager._params = new Dictionary<string, string>
            { 
                ["search"] = search
            };

            Domain.Clients.Hub clientsHub = new Domain.Clients.Hub();
            Resp r = await clientsHub.GetClientsListAysnc(pager);
            return Pack(r);
        }

        /*
        获取用户详情
         */
        [HttpGet("{id}")]
        public async Task<IActionResult> GetClientDetailAsync(int id)
        {
            Domain.Clients.User user = Domain.Clients.Hub.GetUser(id);
            Resp r = await user.GetDetailAsync();
            return Pack(r);
        }

        /// <summary>
        /// 启用用户
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}/enabled")]
        public async Task<IActionResult> EnabledAsync(int id)
        {
            Domain.Clients.User user = Domain.Clients.Hub.GetUser(id);
            Resp r = await user.EnabledAsync();
            return Pack(r);
        }

        /// <summary>
        /// 禁用用户
        /// </summary>
        /// <returns></returns>
        [HttpPatch("{id}/disabled")]
        public async Task<IActionResult> DisabledAsync(int id)
        {
            Domain.Clients.User user = Domain.Clients.Hub.GetUser(id);
            Resp r = await user.DisabledAsync();
            return Pack(r);
        }
    }
}
