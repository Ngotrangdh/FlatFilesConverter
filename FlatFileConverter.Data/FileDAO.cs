using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;

namespace FlatFileConverter.Data
{
    public class FileDAO
    {
        public static void SaveTable(string jsonConfig, int userID, string fileName, DataTable table)
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
            string insertCommandString = "INSERT INTO[dbo].[UserTableMappings] ([UserID],[TableName],[Configuration],[CreatedDate]) VALUES (@userID, @tableName, @configuration, @createdDate);";

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
                    insertCommand.Parameters.AddWithValue("configuration", jsonConfig);
                    insertCommand.Parameters.AddWithValue("createdDate", DateTime.Now);
                    insertCommand.ExecuteNonQuery();
                }
            }
        }
        
        public static List<File> GetFileList(int userID)
        {
            string connectionString = "Server=localhost\\SQLEXPRESS;Database=FlatFilesConverter;Trusted_Connection=True;";
            string selectCommandText = "SELECT [TableID],[UserID],[TableName],[Configuration],[CreatedDate] FROM[dbo].[UserTableMappings] WHERE UserID=@userID";

            List<File> fileList = new List<File>();

            using(SqlConnection conn = new SqlConnection(connectionString))
            {
                using(SqlCommand selectComm = new SqlCommand(selectCommandText, conn))
                {
                    conn.Open();
                    selectComm.Parameters.AddWithValue("userID", userID);

                    using (SqlDataReader reader = selectComm.ExecuteReader())
                    {
                        while(reader.Read())
                        {
                            fileList.Add(new File() 
                                        { FileName = reader.GetString(2), 
                                          FileConfig = reader.GetString(3), 
                                          CreatedDay = reader.GetDateTime(4)
                                        });
                        }
                    }
                }
            }
            return fileList;
        }
    }
}
