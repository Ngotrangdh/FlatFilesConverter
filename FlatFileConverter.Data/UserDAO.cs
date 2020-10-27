using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using System.Text;

namespace FlatFileConverter.Data
{
    public class UserDAO
    {
        public static List<string> ReadDB()
        {
            List<string> lines = new List<string>();
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=FlatFilesConverter;Trusted_Connection=True;";
            string queryString = "select * from dbo.Product;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand selectCommand = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();

                    using (SqlDataReader reader = selectCommand.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            lines.Add(reader[2].ToString());
                        }
                    }
                }

                catch (Exception)
                {

                    throw;
                }
                return lines;
            }
        }

        public static void SaveDataTable(int userID, string fileName, DataTable table)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string tableName = @fileName + '_' + DateTime.Now.Ticks.ToString();
            var columnConfig = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnConfig.Add(column.ColumnName + ' ' + "nvarchar(4000)");
            }
            string columnParam = string.Join(",", columnConfig);
            string createTableCommandText = $"create table {tableName} ({columnParam});";
            string insertCommandString = "INSERT INTO[dbo].[UserTableMapping] ([UserID],[TableName]) VALUES (@userID, @tableName);";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand createTableCommand = new SqlCommand(createTableCommandText, connection))
                {
                    using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                    {
                        createTableCommand.ExecuteNonQuery();
                        foreach (DataColumn column in table.Columns)
                        {
                            bulkCopy.ColumnMappings.Add(column.ColumnName, column.ColumnName);
                        }
                        bulkCopy.DestinationTableName = tableName;

                        try
                        {
                            bulkCopy.WriteToServer(table);
                        }
                        catch (Exception)
                        {

                            throw;
                        }
                    }

                }

                using (SqlCommand insertCommand = new SqlCommand(insertCommandString, connection))
                {
                    insertCommand.Parameters.AddWithValue("userID", userID);
                    insertCommand.Parameters.AddWithValue("tableName", tableName);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }

        public static int IsUserAuthenticated(User user)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommand = $"Select * from dbo.[User] where UserName=@username";
            int userID = 0;
            int userIDFromData = 0;
            string passwordSaltString = null;
            string passwordHashString = null;

            // try catch any exception during connection
            using (SqlConnection conn = new SqlConnection(connectionString))
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

        public static bool RegisterUser(User user)
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


            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommand = $"Select Username from dbo.[User] where Username='{user.Username}'";
            string insertCommand = $"Insert into dbo.[User] values (@username, @salt, @hash)";

            // try catch any exception
            using (SqlConnection conn = new SqlConnection(connectionString))
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

        public static int HasUser(string username)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommand = $"Select UserID from dbo.[User] where Username=@username";
            var userID = 0;
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    comm.Parameters.AddWithValue("username", username);
                    var lookUpResult = comm.ExecuteScalar();
                    if (lookUpResult != null) userID = (int)lookUpResult;
                    return userID;
                }
            }
        }
    }
}

