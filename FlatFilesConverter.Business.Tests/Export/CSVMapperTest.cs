using Xunit;
using System.Collections.Generic;
using System.Data;
using System;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Business.Export;

namespace FlatFilesConverter.Export.Tests
{
    public class CSVMapperTest
    {
        [Fact]
        public void Map()
        {
            var table = new DataTable();
            var config = new Configuration();
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
            var sut = new CSVMapper();

            List<string> lines = sut.Map(table, config);

            Assert.Equal("trang,29,09", lines[0]);
            Assert.Equal("phuong,10,10", lines[1]);

        }
    }
}