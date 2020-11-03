using FlatFilesConverter.Business.Services;
using FlatFilesConverter.Data;
using Moq;
using System;
using System.Collections.Generic;
using System.Data;
using System.Text;
using Xunit;

namespace FlatFilesConverter.Business.Tests.Services
{
    public class FileServiceTest
    {
        [Fact]
        public void SaveTable_CallFileDAOWithCorrectParameters()
        {
            string jSONConfig = "abc";
            int userID = 2;
            string fileName = "test";
            var testTable = new DataTable();
            Mock<IFileDAO> mockFileDAO = new Mock<IFileDAO>();
            mockFileDAO.Setup(fileDAO => fileDAO.SaveTable(jSONConfig, userID, fileName, testTable));

            var sut = new FileService(mockFileDAO.Object);

            sut.SaveTable(jSONConfig, userID, fileName, testTable);

            mockFileDAO.Verify(fileDAO => fileDAO.SaveTable(jSONConfig, userID, fileName, testTable), Times.Once);
        }
    }
}
