// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax.Inlines;
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
            char previous = slice.PeekCharExtra(-1);


            if (!slice.Match("[[_TOC_]]"))
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

            slice.Start = slice.Start + 9;

            int inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

            processor.Inline = new DevOpsTOC
            {
                Span =
                {
                    Start = inlineStart,
                    End = inlineStart + 9
                },
                Line = line,
                Column = column
            };

            return true;


            if (previous.IsWhiteSpaceOrZero())
            {
                slice.NextChar();

                char current = slice.CurrentChar;
                int start = slice.Start;
                int end = start;

                while (current.IsDigit())
                {
                    end = slice.Start;
                    current = slice.NextChar();
                }

                if (current.IsWhiteSpaceOrZero())
                {
                    //inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

                    //processor.Inline = new LinkInline
                    //{
                    //    Span =
                    //    {
                    //        Start = inlineStart,
                    //        End = inlineStart + (end - start) + 1
                    //    },
                    //    Line = line,
                    //    Column = column
                    //};

                    //matchFound = true;
                }
            }

            //return matchFound;
        }
    }
}
