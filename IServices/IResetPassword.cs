using IPMO.Models;

namespace IPMO.IServices
{
    public interface IResetPassword
    {
        // validate pupil by checking in database wether received email link
        //retrieved  email and token from link exit in our database if yes then reset password  
        bool ValidatePupilByTokenEmail(string email,string token);
        //if exit then update password
        bool Reset_Password(ResetPasswordViewModel pwd);
      
    }
}
