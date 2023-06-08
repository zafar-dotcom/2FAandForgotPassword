using IPMO.IServices;
using IPMO.Models;
using IPMO.Models.EmailService;
using Microsoft.AspNetCore.Mvc;
using System.Configuration;
using System.Web;

namespace IPMO.Controllers
{
    public class ForgtPasswordController : Controller
    {

        private readonly IEmailService _emailService;
        private readonly IAuth _auth;
        private readonly IPupilRegistrationService _pupil_service;
        private readonly IResetPassword _resetpassword;
        private readonly IEncrypted _encrypt;
        public ForgtPasswordController(IAuth auth,
            IEmailService emailService,
            IPupilRegistrationService service,
            IResetPassword resetpassword,
            IEncrypted encrypt
            
            )
        {
            _auth = auth;
            _emailService = emailService;
            _pupil_service = service;
            _resetpassword = resetpassword;
            _encrypt = encrypt;

        }
        
        public IActionResult forgotpassword()
        {
            return View();
        }
        [HttpPost]
        public IActionResult forgotpassword(string email)
        {

            if (ModelState.IsValid)
            {
                bool isPupilExit = _pupil_service.ValidatePupilByEmail(email);
                if (isPupilExit == true)
                {
                    string To = email, UserID, Password, SMTPPort, Host;
                    var token = _auth.GenerateTwoFactorTokenAsync(email);
                    //Create URL with above token
                    var encrytedEmail = _encrypt.Encrypt(email);
                    string encodedemail = HttpUtility.UrlEncode(encrytedEmail);
                    var encryptedToken =_encrypt.Encrypt(token);
                    string encodeToken=HttpUtility.UrlEncode(encryptedToken);
                    string decodeemail = HttpUtility.UrlDecode(encodedemail);
                    string decodeToken = HttpUtility.UrlDecode(encodeToken);
                    var dec = _encrypt.Decrypt(decodeemail);
                    var dectoken = _encrypt.Decrypt(decodeToken);
                    var lnkHref = "<a href='" + Url.Action("ResetPassword", "ForgtPassword", new {email= encodedemail, code = encodeToken }, "https") + "'>Reset Password</a>";
                    //HTML Template for Send email
                    string subject = "Your changed password";
                    string body = "<b>Please find the Password Reset Link. </b><br/>" + lnkHref;
                    // Get and set the AppSettings using configuration manager.

                    var message = new Message(new string[] { email }, "Reset Password", body);
                bool issent=  _emailService.SendEmail(message);
                    if (issent == true)
                    {
                        return Ok("Please click on the link we have sent to you on Email");
                    }
                    
                }
               
            }
            return View();
        }
        [HttpGet]
        public IActionResult ResetPassword(string email ,string code)
        {
            string decodemail=HttpUtility.UrlDecode(email);
            string decodeToken = HttpUtility.UrlDecode(code);
            var decryptEmail = _encrypt.Decrypt(decodemail);
            var decryptToken = _encrypt.Decrypt(decodeToken);
            
            if (ModelState.IsValid)
            {
               // check pupil exit on provided token and email sent through email link
                bool IsExit = _resetpassword.ValidatePupilByTokenEmail(decryptEmail, decryptToken);
               if (IsExit == true)
              {
                    ViewBag.Email = decryptEmail;
                   return View();
                    //as user exit ,reset password 

               }

            }
            return BadRequest("not set");
        }


        [HttpPost]
        public IActionResult ResetPassword(ResetPasswordViewModel pwd)
        {
           if(ModelState.IsValid)
            {
              bool Ischanged=  _resetpassword.Reset_Password(pwd);
                if (Ischanged == true)
                {
                    return Ok("Password changed successfully");
                }
            }
            return View();
        }
        //public IActionResult EnterPassword(Reset_Password pwd)
        //{
            
        //}
    }
}
