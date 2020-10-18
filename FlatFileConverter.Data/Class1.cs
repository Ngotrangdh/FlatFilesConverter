using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FlatFileConverter.Data
{
    public class Class1
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
            var columnConfig = new List<string>();
            foreach (DataColumn column in table.Columns)
            {
                columnConfig.Add(column.ColumnName + ' ' + column.DataType.ToString());
               
            }
            string columnParam = string.Join(",", columnConfig);
            string createTable = $"create table User ({columnParam});";

            //string createTable = "create table User (username nvarchar(30), age int);";
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
                    bulkCopy.DestinationTableName = "User";

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

    }
}

