using System.Data;
using System.Collections.Generic;
using FlatFilesConverter.Business.Config;
using FlatFilesConverter.Core.Utililies;

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
                    // try catch exception
                    
                    row[column.FieldName] = line.SubStr(column.ColumnPosition, column.FieldLength);
                }
                table.Rows.Add(row);
            }
            return table;
        }
    }
}