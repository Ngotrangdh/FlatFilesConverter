
using FlatFilesConverter.Business.Config;
using System.Collections.Generic;
using System.Data;

namespace FlatFilesConverter.Business.Import
{
    public class Importer
    {
        private IFileReader _fileReader;
        private IMapper _mapper;

        public Importer(IFileReader fileReader, IMapper mapper)
        {
            _fileReader = fileReader;
            _mapper = mapper;
        }

        public DataTable Import(string filePath, Configuration configuration)
        {
            FileReaderOption option = new FileReaderOption { FilePath = filePath, IsFirstLineHeader = configuration.IsFirstLineHeader };
            List<string> lines = _fileReader.Read(option);
            var table = _mapper.Map(lines, configuration);
            return table;
        }

        // Dependency Injection
    }
}