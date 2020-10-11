using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Import
{
    public class FixedWidthMapper : Mapper, IMapper
    {
        public DataTable Map(List<string> lines, Configuration configuration)
        {
            var table = CreateTableSchema(configuration.ColumnLayouts);

            foreach (string line in lines)
            {
                DataRow row = table.NewRow();
                foreach (ColumnLayout column in configuration.ColumnLayouts)
                {
                    row[column.FieldName] = line.Substring(column.ColumnPosition, column.FieldLength);
                }
                table.Rows.Add(row);
            }

            return table;
        }
    }
}