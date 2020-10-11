using System.Collections.Generic;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Import.Config
{
    public class FixWidthConfig
    {
        public List<ColumnLayout> Columns { get; }

        public FixWidthConfig()
        {
            Columns = new List<ColumnLayout>();
        }

        public void AddColumn(ColumnLayout column)
        {
            Columns.Add(column);
        }
    }
}