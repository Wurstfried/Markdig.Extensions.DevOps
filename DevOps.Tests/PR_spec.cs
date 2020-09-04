// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace PR_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsPRs().Build();

        [Theory]
        [InlineData("!1"             , "<a ")]
        [InlineData(" !23 "          , "<a ")]
        [InlineData("abc !543\n"     , "<a ")]
        [InlineData("*abc* !341 test", "<a ")]
        public void PRs(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsPRs().Build();

        [Theory]
        [InlineData(@"\!1", "<a ")]
        [InlineData(@" \!123 abc", "<a ")]
        [InlineData(@"abc \!6543", "<a ")]
        [InlineData(@"abc \!123 \n abc", "<a ")]
        public void Escaped_PRs(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData(@"!1a", "<a ")]
        [InlineData(@"abc!123", "<a ")]
        [InlineData(@"abc !6543abc", "<a ")]
        [InlineData(@"![test]()", "<a ")]
        public void Concatenated_PRs(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
