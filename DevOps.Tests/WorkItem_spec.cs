// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Markdig.Extensions.DevOps;
using Xunit;

namespace WorkItem_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsWorkItems(new DevOpsLinkOptions("https://dev.azure.com/org/prj")).Build();

        [Theory]
        [InlineData("#9"    , "<a ")]
        [InlineData("#123"  , "<a ")]
        [InlineData("#12345", "<a ")]
        [InlineData("#123\n", "<a ")]
        public void WorkItems(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#9 bla bla"  , "<a ")]
        [InlineData("bla #123 bla", "<a ")]
        [InlineData("bla #12345"  , "<a ")]
        [InlineData("bla #123\n"  , "<a ")]
        public void In_paragraphs(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#Horrid heading with #1234"   , "<a ")]
        [InlineData("# heading with #1234"         , "<a ")]
        [InlineData("##Horrid heading with #1234\n", "<a ")]
        [InlineData("## heading with #1234 bla\n"  , "<a ")]
        public void In_headings(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOps(new DevOpsLinkOptions("https://dev.azure.com/org/prj")).Build();

        [Theory]
        [InlineData("#Horrid"          , "<a ")]
        [InlineData("#Horrid heading"  , "<a ")]
        [InlineData("##Horrid heading" , "<a ")]
        [InlineData("#12Horrid heading", "<a ")]
        [InlineData("##12Horrid"       , "<a ")]
        public void Horrid_headings(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("`#123`"    , "<a ")]
        [InlineData("`#1` 23"   , "<a ")]
        [InlineData("12 `#34`"  , "<a ")]
        [InlineData("12 `34` 56", "<a ")]
        public void In_code_inline(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("```vb\n#123\n```"       , "<a ")]
        [InlineData("```\n abc #123 def\n```", "<a ")]
        [InlineData("```xml\nabc #123\n```"  , "<a ")]
        [InlineData("\n``` py \n#1 bla\n``` ", "<a ")]
        public void In_code_block(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
