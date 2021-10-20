using Microsoft.AspNetCore.Mvc;
namespace Web.Controllers
{
    public class PublicController : Controller
    {
        [Route("access-denined", Name = "SharedNoPermission")]
        public IActionResult AccessDenined()
        {
            return View();
        }
    }
}
