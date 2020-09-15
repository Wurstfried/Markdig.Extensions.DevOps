// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace Images_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsImages().Build();

        [Theory]
        [InlineData("![](src.img =100x200)")]
        [InlineData("![abc](img/source.png =100x)")]
        [InlineData("![Image alt](path/to.img \"Image title\" =x20)")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100x20  )")]
        public void Images_with_fix_size(string md)
        {
            Assert.Contains("<a ", Markdown.ToHtml(md, _pipeline));
        }
    }

    //public class Does_not_parse
    //{
    //    private readonly MarkdownPipeline _pipeline;

    //    public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsImages().Build();
    //}
}
