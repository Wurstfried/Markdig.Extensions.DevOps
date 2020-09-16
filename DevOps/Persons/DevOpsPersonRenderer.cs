// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.DevOps.Persons
{
    class DevOpsPersonRenderer : HtmlObjectRenderer<DevOpsPerson>
    {
        protected override void Write(HtmlRenderer renderer, DevOpsPerson person)
        {
            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<span class=\"").Write(person.Class).Write("\"");
                renderer.Write('>').Write(person.Ref).Write("</span>");
            }
            else
            {
                renderer.Write(person.Prefix).Write(person.Ref);
            }
        }
    }
}
