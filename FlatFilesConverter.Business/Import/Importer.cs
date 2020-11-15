using System;
using System.Collections.Generic;
using System.Data;
using FlatFilesConverter.Business.Config;

namespace FlatFilesConverter.Business.Import
{
    public class Importer
    {
        private readonly IFileReader _fileReader;
        private readonly IMapper _mapper;

        public Importer(IMapper mapper) : this(new FileReader(), mapper)
        { }

        public Importer(IFileReader fileReader, IMapper mapper)
        {
            _fileReader = fileReader ?? throw new ArgumentNullException(nameof(fileReader));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        public DataTable Import(string filePath, Configuration configuration)
        {
            FileReaderOption option = new FileReaderOption { FilePath = filePath, IsFirstLineHeader = configuration.IsFirstLineHeader };
            List<string> lines = _fileReader.Read(option);
            var table = _mapper.Map(lines, configuration);
            return table;
        }
    }
}