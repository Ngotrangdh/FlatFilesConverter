using System.Data;
using System.Collections.Generic;
using FlatFilesConverter.Data;
using System;

namespace FlatFilesConverter.Business.Services
{
    public class FileService
    {
        private readonly IFileDAO _fileDAO;

        public FileService() : this(new FileDAO())
        { }

        public FileService(IFileDAO fileDAO)
        {
            _fileDAO = fileDAO ?? throw new ArgumentNullException(nameof(fileDAO));
        }

        public void SaveTable(string jsonConfig, int userID, string fileName, DataTable table)
        {
            _fileDAO.SaveTable(jsonConfig, userID, fileName, table);
        }

        public List<File> GetFileList(int userID)
        {
            return _fileDAO.GetFileList(userID);
        }

        public DataSet GetFileTable(string fileName)
        {
            return _fileDAO.GetFileTable(fileName);
        }

        public string GetFileConfiguration(string fileName)
        {
            return _fileDAO.GetFileConfiguration(fileName);
        }

        public void DeleteFile(string fileName)
        {
            _fileDAO.DeleteTable(fileName);
        }
    }
}
