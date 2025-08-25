using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RE1_GC_CONTAINERS_TOOL.EXTRACT
{
    internal enum FileFormat
    {
        Null,
        DAT,
        EMD,
        EMG, // ok
        IIDAT, // from DAT
        IIEMD, // from EMD
    }
}
