// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Extensions.DevOps.Headings;
using Markdig.Extensions.DevOps.Links;

namespace Markdig
{
    /// <summary>
    /// Provides extension methods for <see cref="MarkdownPipeline"/> to enable several Markdown extensions.
    /// </summary>
    public static class MarkdownExtensions
    {
        public static MarkdownPipelineBuilder UseDevOps(this MarkdownPipelineBuilder pipeline, DevOpsLinkOptions options)
        {
            return pipeline.UseDevOpsHeadings()
                           .UseDevOpsLinks(options);
        }

        public static MarkdownPipelineBuilder UseDevOpsLinks(this MarkdownPipelineBuilder pipeline, DevOpsLinkOptions options)
        {
            if (!pipeline.Extensions.Contains<DevOpsLinksExtension>())
                pipeline.Extensions.Add(new DevOpsLinksExtension(options));
            
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsHeadings(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsHeadingsExtension>();
            return pipeline;
        }
    }
}