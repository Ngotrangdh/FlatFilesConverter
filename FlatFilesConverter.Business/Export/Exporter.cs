using FlatFilesConverter.Business.Config;
using System.Data;

namespace FlatFilesConverter.Business.Export
{
    public class Exporter
    {
        private IMapper _mapper;
        private IWriter _writer;

        public Exporter(IMapper mapper, IWriter writer)
        {
            _mapper = mapper;
            _writer = writer;
        }

        public void Export(DataTable table, string filePath, Configuration configuration)
        {
            _writer.Write(_mapper.Map(table, configuration), filePath);
        }
    }
}