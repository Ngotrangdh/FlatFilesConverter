using System.Collections.Generic;
using System.IO;

namespace FlatFilesConverter.Business.Import
{
    public class FileReader : IFileReader
    {
        public List<string> Read(FileReaderOption option)
        {
            var lines = new List<string>();
            using (StreamReader reader = File.OpenText(option.FilePath))
            {
                string line = null;

                if (option.IsFirstLineHeader)
                {
                    line = reader.ReadLine();
                }

                while ((line = reader.ReadLine()) != null)
                {
                    lines.Add(line);
                }
            }
            return lines;
        }
    }
}
