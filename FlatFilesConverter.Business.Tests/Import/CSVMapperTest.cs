using Xunit;
using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Import;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Import.Tests
{
    public class CSVMapperTest
    {
        [Fact]
        public void Map()
        {
            var sut = new CSVMapper();
            var lines = new List<string> { "a,12,06", "bc,06,01" };
            var columnConfigs = new List<ColumnLayout>
            {
                new ColumnLayout { FieldName = "name",  ColumnPosition = 0, FieldLength = 1 },
                new ColumnLayout { FieldName = "age",   ColumnPosition = 1, FieldLength = 2 },
                new ColumnLayout { FieldName = "grade", ColumnPosition = 3, FieldLength = 2 },
            };

            var config = new Configuration { ColumnLayouts = columnConfigs, Delimiter = ',' };

            DataTable actualTable = sut.Map(lines, config);

            Assert.Equal(2, actualTable.Rows.Count);
            Assert.Equal("a", actualTable.Rows[0]["name"]);
            Assert.Equal("12", actualTable.Rows[0]["age"]);
            Assert.Equal("06", actualTable.Rows[0]["grade"]);
            Assert.Equal("bc", actualTable.Rows[1]["name"]);
            Assert.Equal("06", actualTable.Rows[1]["age"]);
            Assert.Equal("01", actualTable.Rows[1]["grade"]);
        }
    }
}
