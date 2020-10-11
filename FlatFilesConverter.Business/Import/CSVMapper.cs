using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Import
{
    public class CSVMapper : Mapper, IMapper
    {
        public DataTable Map(List<string> lines, Configuration configuration)
        {
            List<ColumnLayout> columns = configuration.ColumnLayouts;
            var table = CreateTableSchema(columns);

            foreach (var line in lines)
            {
                DataRow row = table.NewRow();
                string[] parts = line.Split(configuration.Delimiter);

                for (var i = 0; i < parts.Length; i++)
                {
                    row[columns[i].FieldName] = parts[i];
                }

                table.Rows.Add(row);
            }
            return table;
        }
    }
}