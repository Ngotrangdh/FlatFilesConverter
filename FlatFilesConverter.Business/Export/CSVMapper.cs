using FlatFilesConverter.Business.Config;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace FlatFilesConverter.Business.Export
{
    public class CSVMapper : IMapper
    {
        public List<string> Map(DataTable table, Configuration configuration)
        {
            List<string> lines = new List<string>();
            string part = null;
            foreach (DataRow row in table.Rows)
            {
                List<string> columns = new List<string>();
                foreach (DataColumn column in table.Columns)
                {
                    part = row[column].ToString().Trim();

                    if (part.Contains("\""))
                    {
                        part = part.Replace("\"", "\"\"");
                    }

                    if (part.Contains(configuration.Delimiter))
                    {
                        part = $"\"{part}\"";
                    }
                    columns.Add(part);
                }

                lines.Add(string.Join($"{configuration.Delimiter}", columns));
            }

            return lines;
        }
    }
}