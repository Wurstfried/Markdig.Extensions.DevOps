using System;
using System.Collections.Generic;
using System.Text;

namespace Markdig.Extensions.DevOps.TOCs
{
    class DevOpsTOC : DevOpsLink
    {
        public DevOpsTOC()
        {
            Prefix = '[';
            Class = "toc-container";
        }
    }
}
