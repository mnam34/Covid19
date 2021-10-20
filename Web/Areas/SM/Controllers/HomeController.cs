using DataAccess;
using Interface;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;
using Web.Controllers;

namespace Web.Areas.SM.Controllers
{
    [Area("SM")]
    [Route("sm")]
    public class HomeController : BaseController
    {
        public HomeController(IRepository repository, DataContext context, IMemoryCache memoryCache) : base(repository, context, memoryCache) { }
        [Route("", Name = "SM_Home_Index")]
        public IActionResult Index()
        {
            return View("/Areas/SM/Views/Home/Index.cshtml");
        }
    }
}
