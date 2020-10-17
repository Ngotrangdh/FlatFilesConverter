using Xunit;
using System.Collections.Generic;
using System.Data;
using Moq;
using System;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;

namespace FlatFilesConverter.Export.Tests
{
    public class ExporterTest
    {
        [Fact]
        public void Export_CallMapperAndWriter()
        {
            var table = new DataTable();
            var filePath = "testPath";
            var config = new Configuration();

            DataColumn[] cols =
            {
                new DataColumn("name", typeof(string)),
                new DataColumn("day", typeof(string)),
                new DataColumn("month", typeof(string)),
            };
            table.Columns.AddRange(cols);
            object[] rows =
            {
                new object[] {"trang ", "29", "09"},
                new object[] {"phuong", "10", "10"},
            };

            foreach (object[] row in rows)
            {
                table.Rows.Add(row);
            }

            // var lines = new List<string> { "trang ,29,09", "phuong,10,10" };
            var lines = new List<string> { "trang 2909", "phuong1010" };

            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map(table, config)).Returns(lines);
            var mockWriter = new Mock<IWriter>();

            var sut = new Exporter(mockMapper.Object, mockWriter.Object);

            sut.Export(table, filePath, config);

            mockMapper.Verify(mapper => mapper.Map(table, config), Times.Once);
            mockWriter.Verify(writer => writer.Write(lines, filePath), Times.Once);

        }
    }
}