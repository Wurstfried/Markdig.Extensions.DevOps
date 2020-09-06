﻿// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;

namespace Markdig.Extensions.DevOps.Persons
{
    class DevOpsPersonsExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            pipeline.InlineParsers.AddIfNotAlready<DevOpsPersonInlineParser>();
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer = renderer as HtmlRenderer;
            ObjectRendererCollection renderers = htmlRenderer?.ObjectRenderers;
            if (renderers == null) return;

            renderers.AddIfNotAlready(new DevOpsPersonRenderer());
        }
    }
}
