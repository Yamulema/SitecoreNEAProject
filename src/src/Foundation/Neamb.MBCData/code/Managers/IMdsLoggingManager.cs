using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Neambc.Neamb.Foundation.MBCData.Managers
{
    public interface IMdsLoggingManager
    {
        void ExecuteMdsLoggingProcess(string productCode,string nameForSession,string typeProcess,string mdsId,string cellCode,string materialId);
    }
}
