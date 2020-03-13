using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain;
using Microsoft.AspNetCore.Mvc;
using yougebug_back.Shared;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

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
            pager.Params = new Dictionary<string, string>
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
            
        }
    }
}
