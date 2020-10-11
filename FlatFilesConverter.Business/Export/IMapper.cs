using FlatFilesConverter.Business.Config;
using System.Collections.Generic;
using System.Data;

namespace FlatFilesConverter.Business.Export
{
    public interface IMapper
    {
        List<string> Map(DataTable table, Configuration configuration);
    }
}