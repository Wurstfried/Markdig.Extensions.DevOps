using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;
using System;
using System.Collections.Generic;
using System.Text;

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
