// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

namespace Markdig.Extensions.DevOps.PRs
{
    class DevOpsPR : DevOpsLink
    {
        public DevOpsPR()
        {
            Prefix = '!';
            Class = "mention-pr";
        }
    }
}
