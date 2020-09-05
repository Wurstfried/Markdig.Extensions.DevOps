// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace TOC_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;
        private readonly string expected = "<div class=\"toc-container\"></div>";

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UsePipeTables().UseDevOpsTOCs().Build();

        [Theory]
        [InlineData("[[_TOC_]]")]
        [InlineData(" [[_TOC_]]")]
        [InlineData(" [[_TOC_]]  ")]
        [InlineData("[[_TOC_]]\n")]
        public void TOCs(string markdownText)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("# [[_TOC_]]")]
        [InlineData("#[[_TOC_]] ")]
        [InlineData("###[[_TOC_]]")]
        [InlineData("### [[_TOC_]]\n")]
        public void In_headings(string markdownText)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("| [[_TOC_]] |\n|--|")]
        public void In_tables(string markdownText)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;
        private readonly string expected = "<div class=\"toc-container\"></div>";

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UsePipeTables().UseDevOpsTOCs().Build();

        [Theory]
        [InlineData("a [[_TOC_]]")]
        [InlineData(" [[_TOC_]] bc")]
        [InlineData("ab[[_TOC_]]c  ")]
        [InlineData("| a [[_TOC_]]|\n|--|")]
        public void In_concats(string markdownText)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
