﻿// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Extensions.DevOps.Headings;
using Markdig.Extensions.DevOps.Images;
using Markdig.Extensions.DevOps.Persons;
using Markdig.Extensions.DevOps.PRs;
using Markdig.Extensions.DevOps.TOCs;
using Markdig.Extensions.DevOps.WorkItems;
using Markdig.Parsers;
using System.Linq;

namespace Markdig
{
    /// <summary>
    /// Provides extension methods for <see cref="MarkdownPipeline"/> to enable several Markdown extensions.
    /// </summary>
    public static class MarkdownExtensions
    {
        public static MarkdownPipelineBuilder UseDevOps(this MarkdownPipelineBuilder pipeline)
        {
            return pipeline.UseAdvancedExtensions()
                           .UseDevOpsHeadings()
                           .UseDevOpsImages()
                           .UseDevOpsMermaid()
                           .UseDevOpsPersons()
                           .UseDevOpsPRs()
                           .UseDevOpsTOCs()
                           .UseDevOpsWorkItems();
        }

        public static MarkdownPipelineBuilder UseDevOpsHeadings(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsHeadingsExtension>();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsImages(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsImagesExtension>();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsMermaid(this MarkdownPipelineBuilder pipeline)
        {
            var o = pipeline.BlockParsers.Find<FencedCodeBlockParser>();
            o.OpeningCharacters = o.OpeningCharacters.Append(':').ToArray();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsPersons(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsPersonsExtension>();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsPRs(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsPRsExtension>();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsTOCs(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready<DevOpsTOCsExtension>();
            return pipeline;
        }

        public static MarkdownPipelineBuilder UseDevOpsWorkItems(this MarkdownPipelineBuilder pipeline)
        {
            pipeline.Extensions.AddIfNotAlready(new DevOpsWorkItemsExtension());
            return pipeline;
        }

    }
}