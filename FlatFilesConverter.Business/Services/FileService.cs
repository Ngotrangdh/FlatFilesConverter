using System.Data;
using System.Collections.Generic;
using FlatFileConverter.Data;

namespace FlatFilesConverter.Business.Services
{
    public class FileService
    {
        private FileDAO FileDAO => new FileDAO();
        
        public void SaveTable(string jsonConfig, int userID, string fileName, DataTable table)
        {
            FileDAO.SaveTable(jsonConfig, userID, fileName, table);
        }

        public List<File> GetFileList(int userID)
        {
            return FileDAO.GetFileList(userID);
        }

        public DataSet GetFileTable(string tableName)
        {
            return FileDAO.GetFileTable(tableName);
        }

        public string GetFileConfiguration(string fileName)
        {
            return FileDAO.GetFileConfiguration(fileName);
        }

        public void DeleteFile(string fileName)
        {
            FileDAO.DeleteTable(fileName);
        }
    }
}
