using System.Collections.Generic;

namespace FlatFilesConverter.Business.Config
{
    public class Configuration
    {
        public List<ColumnLayout> ColumnLayouts { get; set; }
        public char Delimiter { get; set; } = ',';
        public bool IsFirstLineHeader;
    }
}