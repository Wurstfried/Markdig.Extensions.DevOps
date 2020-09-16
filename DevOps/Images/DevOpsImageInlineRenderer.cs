// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.DevOps.Images
{
    public class DevOpsImageInlineRenderer : HtmlObjectRenderer<DevOpsImageInline>
    {
        /// <summary>
        /// Gets or sets the literal string in property rel for links
        /// </summary>
        public string Rel { get; set; }

        protected override void Write(HtmlRenderer renderer, DevOpsImageInline link)
        {
            if (!link.IsImage)
                return;

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(link.IsImage ? "<img src=\"" : "<a href=\"");
                renderer.WriteEscapeUrl(link.GetDynamicUrl != null ? link.GetDynamicUrl() ?? link.Url : link.Url);
                renderer.Write("\"");
                renderer.WriteAttributes(link);
            }

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(" alt=\"");
            }
            var wasEnableHtmlForInline = renderer.EnableHtmlForInline;
            renderer.EnableHtmlForInline = false;
            renderer.WriteChildren(link);
            renderer.EnableHtmlForInline = wasEnableHtmlForInline;
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("\"");
            }

            if (renderer.EnableHtmlForInline && !string.IsNullOrEmpty(link.Title))
            {
                renderer.Write(" title=\"");
                renderer.WriteEscape(link.Title);
                renderer.Write("\"");
            }

            if (!string.IsNullOrWhiteSpace(link.Width))
            {
                renderer.Write(" width=\"");
                renderer.WriteEscape(link.Width);
                renderer.Write("\"");
            }

            if (!string.IsNullOrWhiteSpace(link.Height))
            {
                renderer.Write(" height=\"");
                renderer.WriteEscape(link.Height);
                renderer.Write("\"");
            }

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write(" />");
            }
        }
    }
}