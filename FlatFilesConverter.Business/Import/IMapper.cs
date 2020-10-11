using FlatFilesConverter.Business.Config;
using System.Collections.Generic;
using System.Data;

namespace FlatFilesConverter.Business.Import
{
    public interface IMapper
    {
        DataTable Map(List<string> lines, Configuration configuration);
    }
}