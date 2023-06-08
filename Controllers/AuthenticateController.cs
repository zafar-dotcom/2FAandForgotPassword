using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IPMO.Controllers
{
    public class AuthenticateController : Controller
    {
       

        public AuthenticateController()
        {
          
        }
        public IActionResult Index()
        {
           
            return View();
        }
    }
}
