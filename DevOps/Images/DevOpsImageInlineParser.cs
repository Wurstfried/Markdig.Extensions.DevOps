// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Syntax;
using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.DevOps.Images
{
    class DevOpsImageInlineParser : InlineParser
    {
        public DevOpsImageInlineParser()
        {
            OpeningCharacters = new[] { ']' };
        }

        public override bool Match(InlineProcessor processor, ref StringSlice slice)
        {
            slice.NextChar();
            return processor.Inline != null && TryProcessLinkOrImage(processor, ref slice);
        }

        private bool TryProcessLinkOrImage(InlineProcessor inlineState, ref StringSlice text)
        {
            LinkDelimiterInline openParent = inlineState.Inline.FirstParentOfType<LinkDelimiterInline>();

            if (openParent is null
                || !openParent.IsImage
                || !openParent.IsActive
                || text.CurrentChar != '(')
                return false;

            if (TryParseInlineLink(ref text, out string url, out string title, out SourceSpan linkSpan, out SourceSpan titleSpan, out string width, out string height))
            {
                // Inline Link
                var link = new DevOpsImageInline()
                {
                    Url = HtmlHelper.Unescape(url),
                    Title = HtmlHelper.Unescape(title),
                    IsImage = openParent.IsImage,
                    LabelSpan = openParent.LabelSpan,
                    Width = width,
                    Height = height,
                    UrlSpan = inlineState.GetSourcePositionFromLocalSpan(linkSpan),
                    TitleSpan = inlineState.GetSourcePositionFromLocalSpan(titleSpan),
                    Span = new SourceSpan(openParent.Span.Start, inlineState.GetSourcePosition(text.Start - 1)),
                    Line = openParent.Line,
                    Column = openParent.Column,
                };

                openParent.ReplaceBy(link);
                // Notifies processor as we are creating an inline locally
                inlineState.Inline = link;

                // Process emphasis delimiters
                inlineState.PostProcessInlines(0, link, null, false);

                link.IsClosed = true;

                return true;
            }
            
            return false;
        }

        public static bool TryParseInlineLink(ref StringSlice text, out string link, out string title, out SourceSpan linkSpan, out SourceSpan titleSpan, out string width, out string height)
        {
            // 1. An inline link consists of a link text followed immediately by a left parenthesis (, 
            // 2. optional whitespace,  TODO: specs: is it whitespace or multiple whitespaces?
            // 3. an optional link destination, 
            // 4. an optional link title separated from the link destination by whitespace, 
            // 5. optional whitespace,  TODO: specs: is it whitespace or multiple whitespaces?
            // 6. and a right parenthesis )
            bool isValid = false;
            var c = text.CurrentChar;
            link = null;
            title = null;
            width = null;
            height = null;

            linkSpan = SourceSpan.Empty;
            titleSpan = SourceSpan.Empty;

            // 1. An inline link consists of a link text followed immediately by a left parenthesis (, 
            if (c == '(')
            {
                text.NextChar();
                text.TrimStart();

                var pos = text.Start;
                if (LinkHelper.TryParseUrl(ref text, out link))
                {
                    linkSpan.Start = pos;
                    linkSpan.End = text.Start - 1;
                    if (linkSpan.End < linkSpan.Start)
                    {
                        linkSpan = SourceSpan.Empty;
                    }

                    text.TrimStart(out int spaceCount);
                    var hasWhiteSpaces = spaceCount > 0;

                    c = text.CurrentChar;
                    if (c == ')')
                    {
                        isValid = true;
                    }
                    else if (hasWhiteSpaces)
                    {
                        c = text.CurrentChar;
                        pos = text.Start;
                        if (c == ')')
                        {
                            isValid = true;
                        }
                        else if (LinkHelper.TryParseTitle(ref text, out title))
                        {
                            titleSpan.Start = pos;
                            titleSpan.End = text.Start - 1;
                            if (titleSpan.End < titleSpan.Start)
                            {
                                titleSpan = SourceSpan.Empty;
                            }
                            text.TrimStart();
                            c = text.CurrentChar;

                            if (c == ')')
                            {
                                isValid = true;
                            }
                        }
                    }
                }
            }

            // Get width and height
            if (c != ')')
            {
                if (TryParseFixSize(ref text, out width, out height))
                {
                    isValid = true;
                }
            }



            if (isValid)
            {
                // Skip ')'
                text.NextChar();
                title ??= string.Empty;
            }

            return isValid;
        }

        public static bool TryParseFixSize(ref StringSlice text, out string width, out string height)
        {
            width = null;
            height = null;
            var buffer = StringBuilderCache.Local();
            bool xWasThere = false;
            string widthImg = null;

            char c = text.CurrentChar;
            if (c != '=')
                return false;

            while (true)
            {
                c = text.NextChar();

                if (c.IsDigit())
                {
                    buffer.Append(c);
                    continue;
                }

                if (c == 'x')
                {
                    if (xWasThere)
                        return false;
                    else
                    {
                        widthImg = buffer.ToString();
                        buffer.Clear();
                        xWasThere = true;
                        continue;
                    }
                }

                if (c == ')' || c.IsWhitespace())
                    break;
                
                return false;
            }

            if (!xWasThere)
                return false;

            // Skip whitespaces
            while(c.IsWhitespace())
                c = text.NextChar();

            if (c != ')')
                return false;

            width = widthImg;
            height = buffer.ToString();
            return true;
        }
    }
}
