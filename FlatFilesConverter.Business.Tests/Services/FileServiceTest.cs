using System;
using System.Data;
using System.Collections.Generic;
using FlatFilesConverter.Business.Services;
using FlatFilesConverter.Data;
using Moq;
using Xunit;

namespace FlatFilesConverter.Business.Tests.Services
{
    public class FileServiceTest
    {
        private readonly Mock<IFileDAO> _mockFileDAO;
        private readonly FileService _sut;

        public FileServiceTest()
        {
            _mockFileDAO = new Mock<IFileDAO>();
            _sut = new FileService(_mockFileDAO.Object);
        }

        [Fact]
        public void SaveTable_CallFileDAO_WithCorrectParameters()
        {
            string jSONConfig = "abc";
            int userID = 2;
            string fileName = "test";
            var testTable = new DataTable();
            _mockFileDAO.Setup(fileDAO => fileDAO.SaveTable(jSONConfig, userID, fileName, testTable));

            _sut.SaveTable(jSONConfig, userID, fileName, testTable);

            _mockFileDAO.Verify(fileDAO => fileDAO.SaveTable(jSONConfig, userID, fileName, testTable), Times.Once);
        }

        [Fact]
        public void GetFileList_CallFileDAO_WithCorrectUserID()
        {
            DateTime now = DateTime.Now;
            int userID = 2;

            List<File> files = new List<File>
            {
                new File
                {
                    FileName = "fileTest", 
                    FileConfig = "configTest",
                    CreatedDate = now
                }
            };

            _mockFileDAO.Setup(fileDAO => fileDAO.GetFileList(userID)).Returns(files);

            var actualFiles = _sut.GetFileList(userID);

            Assert.Equal(files, actualFiles);
            _mockFileDAO.Verify(fileDAO => fileDAO.GetFileList(userID), Times.Once);
        }

        [Fact]
        public void GetFileTable_CallFileDAO_WithCorrectFileName()
        {
            string fileName = "test";
            DataSet dataSet = new DataSet();

            _mockFileDAO.Setup(fileDAO => fileDAO.GetFileTable(fileName)).Returns(dataSet);

            var actualDataSet = _sut.GetFileTable(fileName);

            Assert.Equal(dataSet, actualDataSet);
            _mockFileDAO.Verify(fileDAO => fileDAO.GetFileTable(fileName), Times.Once);
        }

        [Fact]
        public void GetFileConfiguration_CallFileDAO_WithFileName()
        {
            string fileName = "test";
            string config = "configTest";

            _mockFileDAO.Setup(fileDAO => fileDAO.GetFileConfiguration(fileName)).Returns(config);
            var actualConfig = _sut.GetFileConfiguration(fileName);

            Assert.Equal(config, actualConfig);
            _mockFileDAO.Verify(fileDAO => fileDAO.GetFileConfiguration(fileName), Times.Once);

        }
    
        [Fact]
        public void DeleteTable_CallFileDAO_WithCorrectFileName()
        {
            string fileName = "test";

            _mockFileDAO.Setup(fileDAO => fileDAO.DeleteTable(fileName));
            _sut.DeleteFile(fileName);
            _mockFileDAO.Verify(fileDAO => fileDAO.DeleteTable(fileName), Times.Once);
        }
    }
}
