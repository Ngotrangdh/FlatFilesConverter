using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Import
{
    public class Mapper
    {
        protected DataTable CreateTableSchema(List<ColumnLayout> columns)
        {
            var table = new DataTable();
            foreach (var column in columns)
            {
                var dataColumn = new DataColumn(column.FieldName, typeof(string));
                table.Columns.Add(dataColumn);
            }
            return table;
        }
    }
}