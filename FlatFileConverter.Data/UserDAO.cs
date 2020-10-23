using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Security.Cryptography;
using System.Text;

namespace FlatFileConverter.Data
{
    public class UserDAO
    {
        public static List<string> readDB()
        {
            List<string> lines = new List<string>();
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=SampleDB;Trusted_Connection=True;";
            string queryString = "select * from SampleDB.dbo.Product;";
            //string nonQueryString = "insert into SampleDB.dbo.Product (ProductName, Price) values ('Mouse', 12);";
            //string deleteQueryString = "delete from SampleDB.dbo.Product where ProductID=4";
            string updateQueryString = "update SampleDB.dbo.Product set price=1 where ProductID=1;";
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand insertCommand = new SqlCommand(updateQueryString, connection);
                SqlCommand selectCommand = new SqlCommand(queryString, connection);
                try
                {
                    connection.Open();
                    insertCommand.ExecuteNonQuery();

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

        public static void saveDataTable(DataTable table)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS; Database=SampleDB;Trusted_Connection=True;";
            string tableName = "trang_" + DateTime.Now.Ticks.ToString();
            var columnConfig = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnConfig.Add(column.ColumnName + ' ' + "nvarchar(4000)");
            }
            string columnParam = string.Join(",", columnConfig);
            string createTable = $"create table {tableName} ({columnParam});";

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(createTable, connection);
                command.ExecuteNonQuery();

                using (SqlBulkCopy bulkCopy = new SqlBulkCopy(connection))
                {
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
            
        }

        public static bool IsUserAuthenticated(User user)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommand = $"Select * from dbo.[User] where UserName='{user.Username}'";
            string passwordSaltString = null;
            string passwordHashString = null;


            // try catch any exception during connection
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommand, conn))
                {
                    conn.Open();
                    var reader = comm.ExecuteReader();

                    if (!reader.HasRows)
                    {
                        return false;
                    }
                    else
                    {
                        while(reader.Read())
                        {
                            passwordSaltString = reader.GetString(2);
                            passwordHashString = reader.GetString(3);
                        }
                        
                        int iterations = 1000;
                        byte[] salt =Convert.FromBase64String(passwordSaltString);
                        var deriveKey = new Rfc2898DeriveBytes(user.Password, salt, iterations);
                        var passwordHashByte = deriveKey.GetBytes(20);
                        var hashToCompare = Convert.ToBase64String(passwordHashByte);

                        if (!string.Equals(passwordHashString, hashToCompare))
                        {
                            return false;
                        }
                        return true;
                    }
                }
            }
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
            using(SqlConnection conn = new SqlConnection(connectionString))
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
    }
}

