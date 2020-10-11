using System.Collections.Generic;
using System.IO;

namespace FlatFilesConverter.Business.Export
{
    public interface IWriter
    {
        void Write(List<string> lines, string filePath);
    }
}