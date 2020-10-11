using System;

namespace FlatFilesConverter.Business.Config
{
    [Serializable]
    public class ColumnLayout
    {
        public string FieldName { get; set; }
        public int ColumnPosition { get; set; }
        public int FieldLength { get; set; }
    }
}