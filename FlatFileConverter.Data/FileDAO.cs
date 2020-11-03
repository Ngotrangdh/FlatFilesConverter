using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;

namespace FlatFilesConverter.Data
{
    public class FileDAO : IFileDAO
    {
        private string ConnectionString => ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;

        public void SaveTable(string jsonConfig, int userID, string fileName, DataTable table)
        {
            string tableName = fileName + '_' + DateTime.Now.Ticks.ToString();
            var columnConfig = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnConfig.Add(column.ColumnName + ' ' + "nvarchar(4000)");
            }
            string columnParam = string.Join(",", columnConfig);
            string createTableCommandText = $"create table {tableName} ({columnParam});";
            string insertCommandString = "INSERT INTO[dbo].[UserTableMappings] ([UserID],[TableName],[Configuration],[CreatedDate]) VALUES (@userID, @tableName, @configuration, @createdDate);";

            using (SqlConnection connection = new SqlConnection(ConnectionString))
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

        public List<File> GetFileList(int userID)
        {
            string selectCommandText = "SELECT [TableID],[UserID],[TableName],[Configuration],[CreatedDate] FROM[dbo].[UserTableMappings] WHERE UserID=@userID";

            List<File> fileList = new List<File>();

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand selectComm = new SqlCommand(selectCommandText, conn))
                {
                    conn.Open();
                    selectComm.Parameters.AddWithValue("userID", userID);

                    using (SqlDataReader reader = selectComm.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            fileList.Add(new File()
                            {
                                FileName = reader.GetString(2),
                                FileConfig = reader.GetString(3),
                                CreatedDate = reader.GetDateTime(4)
                            });
                        }
                    }
                }
            }
            return fileList;
        }

        public DataSet GetFileTable(string tableName)
        {
            string selectCommandText = $"SELECT * FROM [dbo].[{tableName}];";
            var file = new DataSet();
            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlDataAdapter adapter = new SqlDataAdapter(selectCommandText, conn))
                {
                    var mappings = adapter.TableMappings.Add(tableName, tableName);
                    conn.Open();
                    adapter.FillSchema(file, SchemaType.Source);
                    adapter.Fill(file);
                }
            }
            return file;
        }

        public string GetFileConfiguration(string tableName)
        {
            string selectCommandText = $"SELECT Configuration FROM [dbo].[UserTableMappings] WHERE TableName='{tableName}';";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                using (SqlCommand comm = new SqlCommand(selectCommandText, conn))
                {
                    conn.Open();
                    var lookUpResult = comm.ExecuteScalar();

                    // check if lookUpResult equals to null
                    return lookUpResult.ToString();
                }
            }
        }

        public void DeleteTable(string fileName)
        {
            string deleteCommUserTableMappings = "DELETE FROM [dbo].[UserTableMappings] WHERE TableName=@tableName";
            string dropCommTable = $"DROP TABLE[dbo].[{fileName}]";

            using (SqlConnection conn = new SqlConnection(ConnectionString))
            {
                conn.Open();

                using (SqlCommand comm = new SqlCommand(deleteCommUserTableMappings, conn))
                {
                    comm.Parameters.AddWithValue("tableName", fileName);
                    comm.ExecuteNonQuery();
                }

                using (SqlCommand comm = new SqlCommand(dropCommTable, conn))
                {
                    comm.ExecuteNonQuery();
                }
            }

        }
    }
}
