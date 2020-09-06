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
        [InlineData("!1")]
        [InlineData(" !23 ")]
        [InlineData("abc !543\n")]
        [InlineData("*abc* !341 test")]
        public void PRs(string md)
        {
            Assert.Contains("<a ", Markdown.ToHtml(md, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsPRs().Build();

        [Theory]
        [InlineData(@"\!1")]
        [InlineData(@" \!123 abc")]
        [InlineData(@"abc \!6543")]
        [InlineData(@"abc \!123 \n abc")]
        public void Escaped_PRs(string md)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData(@"!1a")]
        [InlineData(@"abc!123")]
        [InlineData(@"abc !6543abc")]
        [InlineData(@"![test]()")]
        public void Concatenated_PRs(string md)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(md, _pipeline));
        }
    }
}
