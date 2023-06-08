using IPMO.Models;

namespace IPMO.IServices
{
    public interface IPupilRegistrationService
    {
        int CheckIsUserExit(PupilRegistration pupil);
      
        bool ValidatePupilByEmail(string email);
    }
}
