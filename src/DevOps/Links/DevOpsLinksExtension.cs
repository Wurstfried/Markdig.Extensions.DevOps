
// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Helpers;
using Markdig.Parsers;
using Markdig.Renderers;

namespace Markdig.Extensions.DevOps.Links
{
    /// <summary>
    /// Markdig markdown extension for DevOps work item linkss
    /// </summary>
    public class DevOpsLinksExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.InlineParsers.Contains<DevOpsLinkInlineParser>())
                pipeline.InlineParsers.Add(new DevOpsLinkInlineParser());
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
        }
    }
}
