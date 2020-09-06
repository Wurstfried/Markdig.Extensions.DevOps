// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace Mermaid_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDiagrams().UseDevOpsMermaid().Build();

        [Theory]
        [InlineData("::: mermaid\ntest\n:::")]
        public void WorkItems(string md)
        {
            Assert.DoesNotContain("<p>", Markdown.ToHtml(md, _pipeline));
        }
    }
}
