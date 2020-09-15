// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Syntax.Inlines;


namespace Markdig.Extensions.DevOps.Images
{
    class DevOpsLinkInline : LinkInline
    {
        public string Width { get; set; }
        public string Height { get; set; }
    }
}
