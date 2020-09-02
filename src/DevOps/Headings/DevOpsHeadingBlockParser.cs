
// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Markdig.Extensions.DevOps.Headings
{
    /// <summary>
    /// Block parser for a <see cref="HeadingBlock"/>.
    /// </summary>
    /// <seealso cref="BlockParser" />
    public class DevOpsHeadingBlockParser : HeadingBlockParser
    {
        public override BlockState TryOpen(BlockProcessor processor)
        {
            // If we are in a CodeIndent, early exit
            if (processor.IsCodeIndent)
            {
                return BlockState.None;
            }

            // DevOps headings
            // A DevOps heading consists of a string of characters, parsed as inline content, 
            // between an opening sequence of 1–6(configurable) unescaped # characters and an optional 
            // closing sequence of any number of unescaped # characters. The opening sequence 
            // of # characters may be followed by a space or by the end of line. The optional
            // closing sequence of #s must be preceded by a space and may be followed by spaces
            // only. The opening # character may be indented 0-3 spaces. The raw contents of 
            // the heading are stripped of leading and trailing spaces before being parsed as 
            // inline content. The heading level is equal to the number of # characters in the 
            // opening sequence.
            var column = processor.Column;
            var line   = processor.Line;
            var sourcePosition = line.Start;
            var c              = line.CurrentChar;
            var matchingChar   = c;

            Debug.Assert(MaxLeadingCount > 0);
            int leadingCount = 0;
            while (c != '\0' && leadingCount <= MaxLeadingCount)
            {
                if (c != matchingChar)
                {
                    break;
                }
                c = line.NextChar();
                leadingCount++;
            }

            // A space is NOT required after leading #
            if (leadingCount > 0 && leadingCount <= MaxLeadingCount)
            {
                Regex re = new Regex(@"^\d+$");
                if (re.IsMatch(line.ToString()))
                    return BlockState.None;

                // Move to the content
                var headingBlock = new HeadingBlock(this)
                {
                    HeaderChar = matchingChar,
                    Level = leadingCount,
                    Column = column,
                    Span = { Start = sourcePosition }
                };
                processor.NewBlocks.Push(headingBlock);
                processor.GoToColumn(column + leadingCount + 1);

                // Gives a chance to parse attributes
                TryParseAttributes?.Invoke(processor, ref processor.Line, headingBlock);

                // The optional closing sequence of #s must be preceded by a space and may be followed by spaces only.
                int endState = 0;
                int countClosingTags = 0;
                for (int i = processor.Line.End; i >= processor.Line.Start - 1; i--)  // Go up to Start - 1 in order to match the space after the first ###
                {
                    c = processor.Line.Text[i];
                    if (endState == 0)
                    {
                        if (c.IsSpaceOrTab())
                        {
                            continue;
                        }
                        endState = 1;
                    }
                    if (endState == 1)
                    {
                        if (c == matchingChar)
                        {
                            countClosingTags++;
                            continue;
                        }

                        if (countClosingTags > 0)
                        {
                            if (c.IsSpaceOrTab())
                            {
                                processor.Line.End = i - 1;
                            }
                            break;
                        }
                        else
                        {
                            break;
                        }
                    }
                }

                // Setup the source end position of this element
                headingBlock.Span.End = processor.Line.End;

                // We expect a single line, so don't continue
                return BlockState.Break;
            }

            // Else we don't have an header
            return BlockState.None;
        }
    }
}
