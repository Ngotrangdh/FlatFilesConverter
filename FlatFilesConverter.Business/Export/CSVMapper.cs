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
            foreach (DataRow row in table.Rows)
            {
                List<string> columns = new List<string>();

                foreach (DataColumn column in table.Columns)
                {
                    columns.Add(row[column].ToString());
                }

                lines.Add(string.Join(",", columns));


                // IEnumerable<string> columns = table.Columns.Cast<DataColumn>().Select(col => row[col].ToString());
                // lines.Add(string.Join(",", columns));
            }

            return lines;
        }
    }
}