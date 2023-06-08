using IPMO.IServices;
using IPMO.Models;
using BCrypt.Net;
using MySql.Data.MySqlClient;
using Org.BouncyCastle.Crypto.Generators;

namespace IPMO.Services
{
    public class PupilRegistrationService : IPupilRegistrationService
    {
        private readonly string str;
        private readonly IEncrypted _encrypt;
         public PupilRegistrationService(IEncrypted encrypt)
        {
            str = "server=localhost;port=3306;uid=root;pwd=sobiazafar@2023;database=IPMO";
            _encrypt = encrypt;

        }

       
        public int CheckIsUserExit(PupilRegistration pupil)
        {
            
            // Generate a random salt value
            string salt = BCrypt.Net.BCrypt.GenerateSalt();
            // Combine the password with salt
            string combinepassword = salt + pupil.Password;
            // Apply bcrypt hashing to generate the hash of combinepassword
            string hashpassword = BCrypt.Net.BCrypt.HashPassword(combinepassword);
            // Store the hashed password and salt in the database
            PupilRegistration obj = new PupilRegistration();
            try
            {
                using (MySqlConnection conn = new MySqlConnection(str))
                {
                    conn.Open();
                    string query = "select count(*) from UserRegistration where Email=@email";
                    using (MySqlCommand cmd1=new MySqlCommand(query, conn))
                    {
                        cmd1.Parameters.AddWithValue("@email", pupil.Email);               
                     int userexit=  Convert.ToInt32(cmd1.ExecuteScalar());
                        conn.Close();
                        if(userexit > 0)
                        {
                            return 1;
                        }

                    }
                    using (MySqlCommand cmd = new MySqlCommand("sp_userregistration", conn))
                    {
                        conn.Open();
                        cmd.CommandType = System.Data.CommandType.StoredProcedure;
                        cmd.Parameters.AddWithValue("_firstname", pupil.FirstName);
                        cmd.Parameters.AddWithValue("_lastname", pupil.LastName);
                        cmd.Parameters.AddWithValue("_password", hashpassword);
                        cmd.Parameters.AddWithValue("_email", pupil.Email);
                        cmd.Parameters.AddWithValue("_phone", pupil.PhoneNumber);                      
                        cmd.Parameters.AddWithValue("_gender", pupil.Gender);
                        cmd.Parameters.AddWithValue("_salt", salt);
                        int rowsaffected = cmd.ExecuteNonQuery();
                        conn.Close();
                        if (rowsaffected > 0)
                        {
                            return 2;
                        }
                        else
                        {
                            return 3;
                        }

                    }
                }
            }
            catch (Exception)
            {

                throw;
            }

        }

        //forgot password ,1st check if user exit on provided email
        public bool ValidatePupilByEmail(string email)
        {
            using (MySqlConnection conn = new MySqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string query = "select count(*) from UserRegistration where Email=@email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {                     
                        cmd.Parameters.AddWithValue("@email", email);                     
                        int rowsreturned = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                        if (rowsreturned > 0)
                        {
                            return true;
                        }
                        else
                        {
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
