using IPMO.IServices;
using IPMO.Models;
using MySql.Data.MySqlClient;

namespace IPMO.Services
{
    public class PupilLoginService : IPupilLogin
    {
        private readonly string str;
        private readonly IEncrypted _encrypt;
        public PupilLoginService(IEncrypted encrypt)
        {
            str = "server=localhost;port=3306;uid=root;pwd=sobiazafar@2023;database=IPMO";
            _encrypt = encrypt;

        }
        public bool PupilValidation(PupilLogin pupil)
        {
            PupilRegistration model = new PupilRegistration();
            using (MySqlConnection conn = new MySqlConnection(str))
            {
                try
                {
                    string fetchQuery = "SELECT Password, salt FROM UserRegistration WHERE Email = @Email";
                    using (MySqlCommand fetchPupil = new MySqlCommand(fetchQuery, conn))
                    {
                        conn.Open();
                        fetchPupil.Parameters.AddWithValue("@Email", pupil.Email);
                        using (MySqlDataReader dr = fetchPupil.ExecuteReader())
                        {
                            if (dr.Read())
                            {
                                string retrievedPassword = dr["Password"].ToString();
                                string retrievedSalt = dr["salt"].ToString();

                                // Combine the provided password and retrieved salt
                                string combinedProvidedPassword = retrievedSalt +pupil.Password;
                                //string hashprovidedpasword = BCrypt.Net.BCrypt.HashPassword(combinedProvidedPassword);
                                // Verify the password
                                bool isPasswordValid = BCrypt.Net.BCrypt.Verify(combinedProvidedPassword, retrievedPassword);

                                if (isPasswordValid)
                                {
                                    return true;
                                }
                            }

                            // No matching rows found or password not valid
                            return false;
                        }
                    }
                }
                catch (Exception)
                {
                    throw;
                }
            }


        }
    }
}
