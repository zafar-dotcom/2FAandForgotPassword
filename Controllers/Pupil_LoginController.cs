using IPMO.IServices;
using IPMO.Models;
using IPMO.Models.EmailService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace IPMO.Controllers
{
    [Route("api/{controller}")]
    public class Pupil_LoginController : Controller
    {
        private readonly IPupilLogin _loginservice;
        private readonly IAuth _auth;
        private readonly IEmailService _emailservice;
        public Pupil_LoginController(IPupilLogin loginservice,
             IAuth auth,
             IEmailService emailservice
            )
        {
            _loginservice = loginservice;           
            _auth = auth;
            _emailservice = emailservice;  
        }
        [HttpGet]
        [Route("login")]
        public IActionResult login()
        {
            return View();
        }
        [HttpPost]
        [Route("login")]
        public async  Task<IActionResult> login(PupilLogin pupil)
        {
            if (ModelState.IsValid)
            {
                bool isPupilExit = _loginservice.PupilValidation(pupil);
                if (isPupilExit == true)
                {
                    
                    //generate token and store into database as well
                    var token = _auth.GenerateTwoFactorTokenAsync(pupil.Email);                
                    var message = new Message(new string[] { pupil.Email! }, "OTP Confirmation", token);
                 //if token has been sent to your email 
                    bool Issent= _emailservice.SendEmail(message);
                    if (Issent == true)
                    {
                        return RedirectToAction("EnterToken");
                    }
                    
                }
                else
                {
                    ModelState.AddModelError("", "Invalid email or password");
                }
            }

            return View();
        }
        [HttpGet]
        public IActionResult EnterToken()
        {
            return View();
        }

        // Recive token from the user and confirm from the database we have already stored
        [HttpPost]
        public IActionResult EnterToken(string token)
            {
            if (ModelState.IsValid)
            {
               bool IsUserExit= _auth.EmailConfirmIfExit(token);
                if (IsUserExit == true)
                {
                    return Ok("user  confirmed successfully");
                }
                else
                {
                    return BadRequest("User not confirmed");
                }
                
            }

            return View();          
              }

    }
}
