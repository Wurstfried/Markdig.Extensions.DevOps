
// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Renderers;

namespace Markdig.Extensions.DevOps.Headings
{
    /// <summary>
    /// Markdig extension for horrid headers in markdown. 
    /// </summary>
    public class DevOpsIDsExtension : IMarkdownExtension
    {
        /// <summary>
        /// Sets up the extension to use the DevOpsHeading block parser
        /// </summary>
        /// <returns>The setup.</returns>
        /// <param name="pipeline">Pipeline.</param>
        public void Setup(MarkdownPipelineBuilder pipeline)
        {
            if (!pipeline.BlockParsers.Contains<DevOpsHeadingBlockParser>())
            {
                // Insert the parser before any other parsers
                pipeline.BlockParsers.Insert(0, new DevOpsHeadingBlockParser());
            }
        }

        /// <summary>
        /// Not needed
        /// </summary>
        /// <returns>The setup.</returns>
        /// <param name="pipeline">Pipeline.</param>
        /// <param name="renderer">Renderer.</param>
        public void Setup(MarkdownPipeline pipeline, IMarkdownRenderer renderer)
        {
            // not required
        }
    }
}
