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

        private bool ProcessLinkReference(InlineProcessor state, string label, bool isShortcut, SourceSpan labelSpan, LinkDelimiterInline parent, int endPosition)
        {
            if (!state.Document.TryGetLinkReferenceDefinition(label, out LinkReferenceDefinition linkRef))
            {
                return false;
            }

            Inline link = null;
            // Try to use a callback directly defined on the LinkReferenceDefinition
            if (linkRef.CreateLinkInline != null)
            {
                link = linkRef.CreateLinkInline(state, linkRef, parent.FirstChild);
            }

            // Create a default link if the callback was not found
            if (link == null)
            {
                // Inline Link
                link = new LinkInline()
                {
                    Url = HtmlHelper.Unescape(linkRef.Url),
                    Title = HtmlHelper.Unescape(linkRef.Title),
                    Label = label,
                    LabelSpan = labelSpan,
                    UrlSpan = linkRef.UrlSpan,
                    IsImage = parent.IsImage,
                    IsShortcut = isShortcut,
                    Reference = linkRef,
                    Span = new SourceSpan(parent.Span.Start, endPosition),
                    Line = parent.Line,
                    Column = parent.Column,
                };
            }

            if (link is ContainerInline containerLink)
            {
                var child = parent.FirstChild;
                if (child == null)
                {
                    child = new LiteralInline()
                    {
                        Content = StringSlice.Empty,
                        IsClosed = true,
                        // Not exact but we leave it like this
                        Span = parent.Span,
                        Line = parent.Line,
                        Column = parent.Column,
                    };
                    containerLink.AppendChild(child);
                }
                else
                {
                    // Insert all child into the link
                    while (child != null)
                    {
                        var next = child.NextSibling;
                        child.Remove();
                        containerLink.AppendChild(child);
                        child = next;
                    }
                }
            }

            link.IsClosed = true;

            // Process emphasis delimiters
            state.PostProcessInlines(0, link, null, false);

            state.Inline = link;

            return true;
        }

        private bool TryProcessLinkOrImage(InlineProcessor inlineState, ref StringSlice text)
        {
            LinkDelimiterInline openParent = inlineState.Inline.FirstParentOfType<LinkDelimiterInline>();

            if (openParent is null || !openParent.IsImage)
                return false;
            

            // If we do find one, but it’s not active,
            // we remove the inactive delimiter from the stack,
            // and return a literal text node ].
            if (!openParent.IsActive)
            {
                inlineState.Inline = new LiteralInline()
                {
                    Content = new StringSlice("["),
                    Span = openParent.Span,
                    Line = openParent.Line,
                    Column = openParent.Column,
                };
                openParent.ReplaceBy(inlineState.Inline);
                return false;
            }

            // If we find one and it’s active,
            // then we parse ahead to see if we have
            // an inline link/image, reference link/image,
            // compact reference link/image,
            // or shortcut reference link/image
            var parentDelimiter = openParent.Parent;
            var savedText = text;

            if (text.CurrentChar == '(')
            {
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

                    // If we have a link (and not an image),
                    // we also set all [ delimiters before the opening delimiter to inactive.
                    // (This will prevent us from getting links within links.)
                    if (!openParent.IsImage)
                    {
                        MarkParentAsInactive(parentDelimiter);
                    }

                    link.IsClosed = true;

                    return true;
                }

                text = savedText;
            }

            var labelSpan = SourceSpan.Empty;
            string label = null;
            bool isLabelSpanLocal = true;

            bool isShortcut = false;
            // Handle Collapsed links
            if (text.CurrentChar == '[')
            {
                if (text.PeekChar() == ']')
                {
                    label = openParent.Label;
                    labelSpan = openParent.LabelSpan;
                    isLabelSpanLocal = false;
                    text.NextChar(); // Skip [
                    text.NextChar(); // Skip ]
                }
            }
            else
            {
                label = openParent.Label;
                isShortcut = true;
            }

            if (label != null || LinkHelper.TryParseLabel(ref text, true, out label, out labelSpan))
            {
                if (isLabelSpanLocal)
                {
                    labelSpan = inlineState.GetSourcePositionFromLocalSpan(labelSpan);
                }

                if (ProcessLinkReference(inlineState, label, isShortcut, labelSpan, openParent, inlineState.GetSourcePosition(text.Start - 1)))
                {
                    // Remove the open parent
                    openParent.Remove();
                    if (!openParent.IsImage)
                    {
                        MarkParentAsInactive(parentDelimiter);
                    }
                    return true;
                }
                else if (text.CurrentChar != ']' && text.CurrentChar != '[')
                {
                    return false;
                }
            }

            // We have a nested [ ]
            // firstParent.Remove();
            // The opening [ will be transformed to a literal followed by all the children of the [

            var literal = new LiteralInline()
            {
                Span = openParent.Span,
                Content = new StringSlice(openParent.IsImage ? "![" : "[")
            };

            inlineState.Inline = openParent.ReplaceBy(literal);
            return false;
        }

        private void MarkParentAsInactive(Inline inline)
        {
            while (inline != null)
            {
                if (inline is LinkDelimiterInline linkInline)
                {
                    if (linkInline.IsImage)
                    {
                        break;
                    }

                    linkInline.IsActive = false;
                }

                inline = inline.Parent;
            }
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
