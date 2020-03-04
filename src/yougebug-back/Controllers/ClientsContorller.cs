using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace yougebug_back.Controllers
{
    /// <summary>
    /// 这是MVC控制器基类
    /// </summary>
    public abstract class ClientsContorller : Controller
    {
        protected void SetTitle(string title)
        {
            if (string.IsNullOrWhiteSpace(title))
                ViewBag.Title = "有个bug，程序员的问答社区";
            else
                ViewBag.Title = title;
        }
    }
}
