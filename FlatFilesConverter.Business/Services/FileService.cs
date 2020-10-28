using System.Collections.Generic;
using System.Data;
using FlatFileConverter.Data;

namespace FlatFilesConverter.Business.Services
{
    public class FileService
    {
        public static void SaveTable(string jsonConfig, int userID, string fileName, DataTable table)
        {
            FileDAO.SaveTable(jsonConfig, userID, fileName, table);
        }

        public static List<File> GetFileList(int userID)
        {
            return FileDAO.GetFileList(userID);
        }
    }
}
