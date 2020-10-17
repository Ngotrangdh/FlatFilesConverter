using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Export
{
    public class FixedWidthMapper : IMapper
    {
        public List<string> Map(DataTable table, Configuration configuration)
        {
            List<string> lines = new List<string>();
            foreach (DataRow row in table.Rows)
            {
                List<string> parts = new List<string>();

                foreach (DataColumn column in table.Columns)
                {
                    parts.Add(row[column].ToString());
                }
                var line = createFixedWidthLine(parts, configuration.ColumnLayouts);
                lines.Add(line);
            }
            return lines;
        }

        private string createFixedWidthLine(List<string> parts, List<ColumnLayout> columns)
        {
            if (parts.Count != columns.Count)
            {
                throw new Exception("Cannot parse file with the given config");
            }
            // var fixedWidthPart = new List<string>();
            // for (var i = 0; i < parts.Count; i++)
            // {
            //     var part = parts[i].PadRight(columns[i].Width, ' ');
            //     fixedWidthPart.Add(part);
            // }
            // var line = string.Join(string.Empty, fixedWidthPart);
            var arr = parts.Select((part, i) => part.PadRight(columns[i].FieldLength, ' '));
            var line = string.Join(string.Empty, arr);
            return line;
        }
    }
}