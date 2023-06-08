using IPMO.Models;

namespace IPMO.IServices
{
    public interface IAuth
    {
        void tokenstoreTodatabase(string emai,string token,DateTime expiration);
        bool EmailConfirmIfExit(string toke);
        string GenerateRandomToken();
       string GenerateTwoFactorTokenAsync(string email);
    }
}
