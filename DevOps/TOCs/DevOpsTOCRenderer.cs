// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.DevOps.TOCs
{
    class DevOpsTOCRenderer : HtmlObjectRenderer<DevOpsTOC>
    {
        protected override void Write(HtmlRenderer renderer, DevOpsTOC link)
        {
            StringSlice itemNumber = link.ItemNumber;

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<div class=\"").Write(link.Class).Write("\"");
                renderer.Write("></div>");
            }
            else
            {
                renderer.Write('#').Write(itemNumber);
            }
        }
    }
}
