// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;
using System.Text.RegularExpressions;

namespace Markdig.Extensions.DevOps.TOCs
{
    /// <summary>
    /// Inline parser for a <see cref="InlineParser"/>.
    /// </summary>
    public class DevOpsTOCInlineParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '['
        };

        public DevOpsTOCInlineParser() => OpeningCharacters = _openingCharacters;

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            string ident = "[[_TOC_]]";

            if (!slice.Match(ident))
                return false;

            // Check before TOC: Allow whitespace, # or |
            StringSlice before = new StringSlice(slice.Text, 0, slice.Start);
            Regex re = new Regex(@"(^\s*#*|\|)\s*$");
            if (!re.IsMatch(slice.Text.Substring(0, slice.Start)))
                return false;

            // Check after TOC
            re = new Regex(@"^\s*(\||$)");
            if (!re.IsMatch(slice.Text.Substring(slice.Start + 9)))
                return false;

            slice.Start += ident.Length;

            int inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

            processor.Inline = new DevOpsTOC
            {
                Span =
                {
                    Start = inlineStart,
                    End = inlineStart + ident.Length
                },
                Line = line,
                Column = column
            };

            return true;
        }
    }
}
