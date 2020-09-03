// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;

namespace Markdig.Extensions.DevOps.Links
{
    /// <summary>
    /// Markdig markdown extension for DevOps work item links
    /// </summary>
    public class DevOpsLinksExtension : IMarkdownExtension
    {
        private readonly DevOpsLinkOptions _options;

        public DevOpsLinksExtension() => _options = new DevOpsLinkOptions();

        public DevOpsLinksExtension(DevOpsLinkOptions options) => _options = options;

        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<DevOpsLinkInlineParser>())
                pipeline.InlineParsers.Add(new DevOpsLinkInlineParser());
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            HtmlRenderer htmlRenderer = renderer as HtmlRenderer;
            ObjectRendererCollection renderers = htmlRenderer?.ObjectRenderers;

            if (renderers != null && !renderers.Contains<DevOpsLinkRenderer>())
            {
                renderers.Add(new DevOpsLinkRenderer(_options));
            }
        }
    }
}
