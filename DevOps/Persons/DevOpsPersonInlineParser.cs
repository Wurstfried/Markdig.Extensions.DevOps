// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;

namespace Markdig.Extensions.DevOps.Persons
{
    class DevOpsPersonInlineParser : InlineParser
    {
        private static readonly char[] _openingCharacters =
        {
            '@'
        };

        public DevOpsPersonInlineParser() => OpeningCharacters = _openingCharacters;

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            char current = slice.NextChar();
            char[] validChars = "!()_ -{ }[]\\".ToCharArray();

            if (current != '<') return false;
            current = slice.NextChar();
            int start = slice.Start;
            int end = start;


            while (current.IsAlphaNumeric()
                || current.IsWhitespace()
                || validChars.Contains(current))
            {
                end++;
                current = slice.NextChar();
            }

            if (current != '>') return false;
            current = slice.NextChar();

            int inlineStart = processor.GetSourcePosition(slice.Start, out int line, out int column);

            processor.Inline = new DevOpsPerson
            {
                Span =
                {
                    Start = inlineStart,
                    End = inlineStart + (end - start) + 1
                },
                Line = line,
                Column = column,
                Ref = new StringSlice(slice.Text, start, end)
            };

            return true;
        }
    }
}
