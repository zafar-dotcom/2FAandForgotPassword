using IPMO.IServices;
using IPMO.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using MySql.Data.MySqlClient;

namespace IPMO.Services
{
    public class Auth : IAuth
    {
        private readonly string str;       
        public Auth()
        {           
            
            str = "server=localhost;port=3306;uid=root;pwd=sobiazafar@2023;database=IPMO";
        }
       public  string GenerateTwoFactorTokenAsync(string email)
        {
            //generate random token
            string token = GenerateRandomToken();
            //set expiration time
            DateTime expiration = DateTime.UtcNow.AddMinutes(3);
            //store token into database
            tokenstoreTodatabase(email,token,expiration);
            return token;

        }
        public string GenerateRandomToken()
        {
            //return Guid.NewGuid().ToString();
            Random random = new Random();
            int randomNumber = random.Next(1000, 9999); // Generate a random number between 1000 and 9999
            return randomNumber.ToString();
        }
        public bool EmailConfirmIfExit(string token)
        {
            using(MySqlConnection conn=new MySqlConnection(str))
            {
                try
                {
                    conn.Open();
                    string query = "select count(*) from twofactorauth where code=@token AND createdat >= @expiration";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.AddWithValue("token", token);
                        cmd.Parameters.AddWithValue("@expiration", DateTime.UtcNow);
                        int rowsaffected = Convert.ToInt32(cmd.ExecuteScalar());

                        if (rowsaffected > 0)
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

        public  void tokenstoreTodatabase(string email,string token ,DateTime expiration)
        {

            

            using (MySqlConnection conn=new MySqlConnection(str))
            {
                try
                {
                    string query1 = "select ID from UserRegistration where Email=@email";
                    conn.Open();
                    using (MySqlCommand cmd_userid = new MySqlCommand(query1, conn))
                    {
                        cmd_userid.Parameters.AddWithValue("@email", email);
                        object result = cmd_userid.ExecuteScalar();
                        conn.Close();
                        if (result != null)
                        {
                            string query = "insert into twofactorauth (code,createdat,userid)" +
                        " values (@code,@createdat,@userid)";
                            using (MySqlCommand storetoken = new MySqlCommand(query, conn))
                            {
                                conn.Open();
                                storetoken.Parameters.AddWithValue("@code", token);
                                storetoken.Parameters.AddWithValue("@createdat", expiration);
                                storetoken.Parameters.AddWithValue("@userid", result);
                                storetoken.ExecuteNonQuery();
                                conn.Close();

                            }

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
