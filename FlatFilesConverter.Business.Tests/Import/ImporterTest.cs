using System.Data;
using System.Collections.Generic;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Import;
using Moq;
using Xunit;

namespace FlatFilesConverter.Import.Tests
{
    public class ImporterTest
    {
        [Fact]
        public void Import()
        {
            var filePath = "testPath";
            var lines = new List<string> { "trang 2909", "phuong1010" };
            var columns = new List<ColumnLayout>
            {
                new ColumnLayout{ FieldName = "name", ColumnPosition = 0, FieldLength = 6},
                new ColumnLayout{ FieldName = "day", ColumnPosition = 6, FieldLength = 2},
                new ColumnLayout{ FieldName = "month", ColumnPosition = 8, FieldLength = 2}
            };
            var config = new Configuration { ColumnLayouts = columns };

            FileReaderOption option = new FileReaderOption { FilePath = filePath, IsFirstLineHeader = false };

            var expectedTable = new DataTable();
            DataColumn[] cols =
            {
                new DataColumn("name", typeof(string)),
                new DataColumn("day", typeof(string)),
                new DataColumn("month", typeof(string)),
            };
            expectedTable.Columns.AddRange(cols);
            object[] rows =
            {
                new [] { "trang ", "29", "09" },
                new [] { "phuong", "10", "10" },
            };
            foreach (object[] row in rows)
            {
                expectedTable.Rows.Add(row);
            }

            var mockFileReader = new Mock<IFileReader>();
            mockFileReader.Setup(reader => reader.Read(It.IsAny<FileReaderOption>())).Returns(lines).Verifiable();
            var mockMapper = new Mock<IMapper>();
            mockMapper.Setup(mapper => mapper.Map(lines, config)).Returns(expectedTable).Verifiable();

            var sut = new Importer(mockFileReader.Object, mockMapper.Object);

            var actualTable = sut.Import(filePath, config);

            mockFileReader.Verify();
            mockMapper.Verify();
        }
    }

}