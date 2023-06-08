using IPMO.IServices;
using IPMO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Security;

namespace IPMO.Controllers
{
    [Route("api/{controller}")]
    public class PupilRegistrationController : Controller
    {
        private readonly IPupilRegistrationService _dal;
      
        public PupilRegistrationController(
            IPupilRegistrationService registrationservices       
            )
        {
            _dal = registrationservices;
            
        }
        [HttpGet]
        [Route("Register")]
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]
        [Route("Register")]
        public async Task<IActionResult> Register(PupilRegistration pupil)
        {
            //int isuserExit = _dal.PupilRegister(pupil);
            //if (isuserExit == 1)
            //{
            //    ModelState.AddModelError("", "User already exit");
            //}
            //IdentityUser user = new()
            //{
            //    Email = pupil.Email,
            //    SecurityStamp = Guid.NewGuid().ToString(),
            //    UserName = pupil.FirstName,
            //    TwoFactorEnabled = true
            //};
            //var token = await _usermanager.GenerateEmailConfirmationTokenAsync(user);
           
            if (ModelState.IsValid)
            {
                
                int result = _dal.CheckIsUserExit(pupil);
                
                if (result == 1)
                {
                    ModelState.AddModelError("", "User already exit");
                }
                else if(result == 2) {
                    return RedirectToAction("login", "Pupil_Login");
                }
               else
                {
                    ModelState.AddModelError("Email", "User Registration failed");
                }
            }
            return View();
        }
       
        


    }
}
