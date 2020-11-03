using System.Collections.Generic;
using System.Data;

namespace FlatFilesConverter.Data
{
    public interface IFileDAO
    {
        void DeleteTable(string fileName);
        string GetFileConfiguration(string tableName);
        List<File> GetFileList(int userID);
        DataSet GetFileTable(string tableName);
        void SaveTable(string jsonConfig, int userID, string fileName, DataTable table);
    }
}