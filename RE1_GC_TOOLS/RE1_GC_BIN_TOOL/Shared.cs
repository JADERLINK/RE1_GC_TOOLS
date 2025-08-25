using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RE1_GC_BIN_TOOL
{
    public static class Shared
    {
        private const string VERSION = "V.1.0.0 (2025-08-24)";

        public static string HeaderText()
        {
            return "# github.com/JADERLINK/RE1_GC_TOOLS" + Environment.NewLine +
                   "# youtube.com/@JADERLINK" + Environment.NewLine +
                   "# RE1_GC_BIN_TOOL by: JADERLINK" + Environment.NewLine +
                   "# Thanks to \"mariokart64n\"" + Environment.NewLine +
                   "# Material information by \"Albert\"" + Environment.NewLine +
                  $"# Version {VERSION}";
        }

        public static string HeaderTextSmd()
        {
            return "// RE1_GC_BIN_TOOL" + Environment.NewLine +
                   "// by: JADERLINK" + Environment.NewLine +
                   "// youtube.com/@JADERLINK" + Environment.NewLine +
                  $"// Version {VERSION}";
        }
    }
}
