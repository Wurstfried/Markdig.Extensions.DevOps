// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.DevOps
{
    public class DevOpsLinkRenderer : HtmlObjectRenderer<DevOpsLink>
    {
        protected override void Write(HtmlRenderer renderer, DevOpsLink link)
        {
            StringSlice itemNumber = link.Ref;

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<a href=\"").Write(itemNumber).Write("\"");
                renderer.Write(" class=\"").Write(link.Class).Write("\"");
                renderer.Write('>').Write(itemNumber).Write(" </a>");
            }
            else
            {
                renderer.Write('#').Write(itemNumber);
            }
        }
    }
}
