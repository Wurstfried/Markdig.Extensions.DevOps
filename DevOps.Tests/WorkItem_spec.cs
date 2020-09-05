// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace WorkItem_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsWorkItems().Build();

        [Theory]
        [InlineData("#9"    )]
        [InlineData("#123"  )]
        [InlineData("#12345")]
        [InlineData("#123\n")]
        public void WorkItems(string markdownText)
        {
            Assert.Contains("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#9 bla bla"  )]
        [InlineData("bla #123 bla")]
        [InlineData("bla #12345"  )]
        [InlineData("bla #123\n"  )]
        public void In_paragraphs(string markdownText)
        {
            Assert.Contains("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#Horrid heading with #1234"   )]
        [InlineData("# heading with #1234"         )]
        [InlineData("##Horrid heading with #1234\n")]
        [InlineData("## heading with #1234 bla\n"  )]
        public void In_headings(string markdownText)
        {
            Assert.Contains("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsWorkItems().Build();

        [Theory]
        [InlineData("#Horrid"          )]
        [InlineData("#Horrid heading"  )]
        [InlineData("##Horrid heading" )]
        [InlineData("#12Horrid heading")]
        [InlineData("##12Horrid"       )]
        public void Horrid_headings(string markdownText)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData(@"\#1"             )]
        [InlineData(@" \#123 abc"      )]
        [InlineData(@"abc \#6543"      )]
        [InlineData(@"abc \#123 \n abc")]
        public void Escaped_workItems(string markdownText)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("`#123`"    )]
        [InlineData("`#1` 23"   )]
        [InlineData("12 `#34`"  )]
        [InlineData("12 `34` 56")]
        public void In_code_inline(string markdownText)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("```vb\n#123\n```"       )]
        [InlineData("```\n abc #123 def\n```")]
        [InlineData("```xml\nabc #123\n```"  )]
        [InlineData("\n``` py \n#1 bla\n``` ")]
        public void In_code_block(string markdownText)
        {
            Assert.DoesNotContain("<a ", Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
