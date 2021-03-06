﻿// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

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
