// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Syntax.Inlines;
using System.Diagnostics;

namespace Markdig.Extensions.DevOps
{
    [DebuggerDisplay("{" + nameof(Prefix) + nameof(ItemNumber) + "}")]
    public abstract class DevOpsLink : LeafInline
    {
        public StringSlice ItemNumber { get; set; }
        public char Prefix { get; set; }
        public string Class { get; set; }
    }
}
