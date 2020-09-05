// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;

namespace Markdig.Extensions.DevOps.PRs
{
    /// <summary>
    /// Inline parser for a <see cref="InlineParser"/>.
    /// </summary>
    public class DevOpsPRInlineParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '!'
        };

        public DevOpsPRInlineParser() => OpeningCharacters = _openingCharacters;

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            bool matchFound = false;
            char previous = slice.PeekCharExtra(-1);

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
                    int inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

                    processor.Inline = new DevOpsPR
                    {
                        Span =
                        {
                            Start = inlineStart,
                            End = inlineStart + (end - start) + 1
                        },
                        Line = line,
                        Column = column,
                        ItemNumber = new StringSlice(slice.Text, start, end)
                    };

                    matchFound = true;
                }
            }

            return matchFound;
        }
    }
}
