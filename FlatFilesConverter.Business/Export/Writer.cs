using System.Collections.Generic;
using System.IO;

namespace FlatFilesConverter.Business.Export
{
    public class Writer : IWriter
    {
        public void Write(List<string> lines, string filePath)
        {
            using (StreamWriter writer = File.AppendText(filePath))
            {
                foreach (var line in lines)
                {
                    writer.WriteLine(line);
                }
            }
        }
    }
}