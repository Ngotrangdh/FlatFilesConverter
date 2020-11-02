using System;
using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;
using Xunit;

namespace FlatFilesConverter.Export.Tests
{
    public class FixedWidthMapperTest
    {
        [Fact]
        public void MapWhenConfigValidAndInOrder()
        {
            var table = new DataTable();
            DataColumn[] cols = {
                                    new DataColumn("name", typeof(string)),
                                    new DataColumn("day", typeof(string)),
                                    new DataColumn("month", typeof(string)),
                                };
            table.Columns.AddRange(cols);
            object[] rows = {
                                new object[] {"trang ", "29", "09"},
                                new object[] {"phuong", "10", "10"},
                            };
            foreach (object[] row in rows)
            {
                table.Rows.Add(row);
            }

            var columnConfigs = new List<ColumnLayout>
            {
                new ColumnLayout { FieldName = "name", ColumnPosition = 0, FieldLength = 6 },
                new ColumnLayout { FieldName = "day", ColumnPosition = 6, FieldLength = 2 },
                new ColumnLayout { FieldName = "month", ColumnPosition = 8, FieldLength = 2 },
            };
            var config = new Configuration { ColumnLayouts = columnConfigs };

            var sut = new FixedWidthMapper();

            List<string> lines = sut.Map(table, config);

            Assert.Equal("trang 2909", lines[0]);
            Assert.Equal("phuong1010", lines[1]);
        }

        [Fact]
        public void ThrowExceptionWhenConfigInvalid()
        {
            var table = new DataTable();
            DataColumn[] cols = {
                                    new DataColumn("name", typeof(string)),
                                    new DataColumn("day", typeof(string)),
                                    new DataColumn("month", typeof(string)),
                                };
            table.Columns.AddRange(cols);
            object[] rows = {
                                new object[] {"trang ", "29", "09"},
                                new object[] {"phuong", "10", "10"},
                            };
            foreach (object[] row in rows)
            {
                table.Rows.Add(row);
            }

            var columnConfigs = new List<ColumnLayout>
            {
                new ColumnLayout { FieldName = "day", ColumnPosition = 6, FieldLength = 2 },
                new ColumnLayout { FieldName = "month", ColumnPosition = 8, FieldLength = 2 },
                new ColumnLayout { FieldName = "year", ColumnPosition = 10, FieldLength = 2 },
                new ColumnLayout { FieldName = "name", ColumnPosition = 0, FieldLength = 6 },
            };
            var config = new Configuration { ColumnLayouts = columnConfigs };

            var sut = new FixedWidthMapper();

            Assert.Throws<Exception>(() => sut.Map(table, config));
        }
    }
}