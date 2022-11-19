using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Neambc.Neamb.Foundation.Rakuten.Model;

namespace Neambc.Neamb.Feature.Rakuten.Repositories
{
    public interface ICategoryImportRepository {
        CategoryImportResult ExecuteImportProcess(Stream stream);
    }
}
