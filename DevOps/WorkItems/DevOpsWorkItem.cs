// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Syntax.Inlines;
using System.Diagnostics;

namespace Markdig.Extensions.DevOps.WorkItems
{
    [DebuggerDisplay("{" + nameof(Prefix) + nameof(IssueNumber) + "}")]
    public class DevOpsWorkItem : LeafInline
    {
        public StringSlice IssueNumber { get; set; }
        public char Prefix { get; set; } = '#';
        public string Class { get; set; } = "mention-workitem";
    }
}
