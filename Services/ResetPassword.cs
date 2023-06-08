using IPMO.IServices;
using IPMO.Models;
using MySql.Data.MySqlClient;

namespace IPMO.Services
{
    public class ResetPassword : IResetPassword
    {
        private readonly string str;
        public ResetPassword()
        {
            str = "server=localhost;port=3306;uid=root;pwd=sobiazafar@2023;database=IPMO";


        }
        public bool Reset_Password(ResetPasswordViewModel pwd)
        {          
                using (MySqlConnection conn = new MySqlConnection(str))
                {
                try
                {
                    conn.Open();
                    string resetpassword = "update UserRegistration set Password=@Password where Email=@email";
                    using (MySqlCommand cmd = new MySqlCommand(resetpassword, conn))
                    {
                        cmd.Parameters.AddWithValue("@Password", pwd.Password);
                        cmd.Parameters.AddWithValue("@email", pwd.Email);
                        int isChanged = cmd.ExecuteNonQuery();
                      
                        if (isChanged > 0)
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
                finally
                {
                    if (conn.State == System.Data.ConnectionState.Open)
                    {
                        conn.Close();
                    }
                   
                }
                }
            }
           
                

        public bool ValidatePupilByTokenEmail(string email,string token)
        {
            try
            {
                using (MySqlConnection conn = new MySqlConnection(str))
                {
                    conn.Open();
                    string query = "select ID from UserRegistration where Email=@email";
                    using (MySqlCommand cmd = new MySqlCommand(query, conn))
                    {
                        cmd.Parameters.Clear();
                        cmd.Parameters.AddWithValue("@email", email);
                        int returnedID = Convert.ToInt32(cmd.ExecuteScalar());
                        conn.Close();
                        if (returnedID > 0)
                        {
                            conn.Open();
                            string checkPupilExit = "select count(*) from twofactorauth where code=@token AND userid=@userid AND createdat >=@expiration";
                            using (MySqlCommand validateByEmailToken = new MySqlCommand(checkPupilExit, conn))
                            {
                                validateByEmailToken.Parameters.AddWithValue("@token", token);
                                validateByEmailToken.Parameters.AddWithValue("@userid", returnedID);
                                validateByEmailToken.Parameters.AddWithValue("@expiration", DateTime.UtcNow);
                                int isPupilExit = Convert.ToInt32(validateByEmailToken.ExecuteScalar());

                                conn.Close();
                                if (isPupilExit == 1)
                                {
                                    //now we can change the password if pupil exit
                                    return true;
                                }
                                else
                                {
                                    return false;
                                }
                            }
                        }
                        else
                        {
                            return false;
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
