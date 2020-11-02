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
                var line = CreateFixedWidthLine(parts, configuration.ColumnLayouts);
                lines.Add(line);
            }
            return lines;
        }

        private string CreateFixedWidthLine(List<string> parts, List<ColumnLayout> columns)
        {
            //TODO: test unordered list
            List<ColumnLayout> sortedColumns = columns.OrderBy(c => c.ColumnPosition).ToList();
            
            //if (parts.Count != columns.Count)
            //{
            //    throw new Exception("Cannot parse file with the given config");
            //}

            var arr = parts.Select((part, i) => part.PadRight(sortedColumns[i].FieldLength, ' '));
            var line = string.Join(string.Empty, arr);
            return line;
        }
    }
}