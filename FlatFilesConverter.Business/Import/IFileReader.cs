using System.Collections.Generic;

namespace FlatFilesConverter.Business.Import
{
    public interface IFileReader
    {
        List<string> Read(FileReaderOption option);
    }
}