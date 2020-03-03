using Microsoft.AspNetCore.Mvc;

namespace yougebug_back.Controllers
{
    [Route("/")]
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            return Redirect("questions");
        }
    }
}