// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;

namespace Markdig.Extensions.DevOps.Headings
{
    /// <summary>
    /// Markdig extension for horrid headers in markdown. 
    /// </summary>
    public class DevOpsHeadingsExtension : IMarkdownExtension
    {
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            var parsers = pipeline.BlockParsers;
            if (!parsers.Contains<DevOpsHeadingBlockParser>())
                parsers.Add(new DevOpsHeadingBlockParser());
        }

        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // nothing to do
        }
    }
}
