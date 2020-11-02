using System;
using System.Configuration;
using System.Data.SqlClient;
using System.Security.Cryptography;

namespace FlatFilesConverter.Data
{
    public class UserDAO
    {
        private string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"].ToString();
        
        public int IsUserAuthenticated(User user)
        {
            string selectCommand = $"Select * from dbo.[User] where UserName=@username";
            int userID = 0;
            int userIDFromData = 0;
            string passwordSaltString = null;
            string passwordHashString = null;

            // try catch any exception during connection
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    comm.Parameters.AddWithValue("username", user.Username);
                    var reader = comm.ExecuteReader();

                    if (reader.HasRows)
                    {
                        // only one line, does it need to use while
                        while (reader.Read())
                        {
                            //use tryParse to catch exception
                            userIDFromData = reader.GetInt32(0);
                            passwordSaltString = reader.GetString(2);
                            passwordHashString = reader.GetString(3);
                        }

                        int iterations = 1000;
                        byte[] salt = Convert.FromBase64String(passwordSaltString);
                        var deriveKey = new Rfc2898DeriveBytes(user.Password, salt, iterations);
                        var passwordHashByte = deriveKey.GetBytes(20);
                        var hashToCompare = Convert.ToBase64String(passwordHashByte);

                        if (string.Equals(passwordHashString, hashToCompare))
                        {
                            userID = userIDFromData;
                        }
                    }
                }
            }
            return userID;
        }

        public bool RegisterUser(User user)
        {
            int iterations = 1000;
            byte[] passwordSalt = new byte[8];
            using (RNGCryptoServiceProvider rngCsp = new RNGCryptoServiceProvider())
            {
                rngCsp.GetBytes(passwordSalt);
            }
            var deriveKey = new Rfc2898DeriveBytes(user.Password, passwordSalt, iterations);
            var passwordHash = deriveKey.GetBytes(20);
            var salt = Convert.ToBase64String(passwordSalt);
            var hash = Convert.ToBase64String(passwordHash);


            string selectCommand = $"Select Username from dbo.[User] where Username='{user.Username}'";
            string insertCommand = $"Insert into dbo.[User] values (@username, @salt, @hash)";

            // try catch any exception
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand selectComm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    var lookUpError = selectComm.ExecuteScalar();
                    if (lookUpError != null)
                    {
                        return false;
                    }
                    else
                    {
                        using (SqlCommand insertComm = new SqlCommand(insertCommand, conn))
                        {
                            insertComm.Parameters.AddWithValue("username", user.Username);
                            insertComm.Parameters.AddWithValue("salt", salt);
                            insertComm.Parameters.AddWithValue("hash", hash);
                            insertComm.ExecuteNonQuery();
                        }
                        return true;
                    }
                }


            }
        }

        public User GetUser(string username)
        {
            string selectCommand = $"Select * from dbo.[User] where Username=@username";
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    comm.Parameters.AddWithValue("username", username);
                    var reader = comm.ExecuteReader();

                    return reader.Read()
                    ? new User { UserID = reader.GetInt32(0), Username = reader.GetString(1), Password = reader.GetString(2) }
                    : null;
                }
            }
        }
    }
}

