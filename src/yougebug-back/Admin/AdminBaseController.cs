using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace yougebug_back.Admin
{
    
    [Authorize]
    public abstract class AdminBaseController : Shared.YGBBaseController
    {
        
    }
}
