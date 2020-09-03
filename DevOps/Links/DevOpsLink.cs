// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Syntax.Inlines;
using System.Diagnostics;

namespace Markdig.Extensions.DevOps.Links
{
    [DebuggerDisplay("#{" + nameof(IssueNumber) + "}")]
    public class DevOpsLink : LeafInline
    {
        public StringSlice IssueNumber { get; set; }
    }
}
