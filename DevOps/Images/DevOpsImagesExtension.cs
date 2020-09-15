// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Parsers.Inlines;
using Markdig.Renderers;
using Markdig.Renderers.Html.Inlines;
using Markdig.Renderers.Normalize;

namespace Markdig.Extensions.DevOps.Images
{
    class DevOpsImagesExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.InlineParsers.InsertBefore<LinkInlineParser>(new DevOpsImageInlineParser());
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer = renderer as HtmlRenderer;
            ObjectRendererCollection renderers = htmlRenderer?.ObjectRenderers;
            if (renderers == null) return;

            renderers.InsertBefore<LinkInlineRenderer>(new DevOpsImageInlineRenderer());
        }
    }
}
