﻿using Markdig.Helpers;
using Markdig.Syntax.Inlines;
using System.Diagnostics;

namespace Markdig.Extensions.DevOps.Links
{
    [DebuggerDisplay("#{" + nameof(IssueNumber) + "}")]
    public class DevOpsLink : LinkInline
    {
        public StringSlice IssueNumber { get; set; }
    }
}