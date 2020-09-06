// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Xunit;

namespace Person_spec
{
    public class Parses
    {
        MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsPersons().Build();

        [Theory]
        [InlineData("@<Sebastian Raffel>")]
        [InlineData("@<Gustav Mayer Gundron> \n")]
        [InlineData("test@<Sebastian Raffel>")]
        [InlineData("@<Sebastian Raffel>test")]
        [InlineData("test@<Sebastian Raffel>test")]
        public void Persons(string markdownText)
        {
            Assert.Contains("<span class=\"mention-person\"", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData(@"@<[FirstProject]\Build Administrators>")]
        [InlineData(@"@<[FirstProject]\Build Administrators> \n")]
        [InlineData(@"test@<[FirstProject]\Build Administrators>")]
        [InlineData(@"@<[FirstProject]\Build Administrators>test")]
        [InlineData(@"test@<[FirstProject]\Build Administrators>test")]
        public void Groups(string markdownText)
        {
            Assert.Contains("<span class=\"mention-person\"", Markdown.ToHtml(markdownText, _pipeline));
        }
    }

    public class Does_not_parse
    {
        MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsPersons().Build();

        [Theory]
        [InlineData("`@<Sebastian Raffel>`")]
        [InlineData("```vb\n@<Gustav Mayer Gundron> \n```")]
        [InlineData(@"`@<[FirstProject]\Build Administrators>`")]
        [InlineData("```cs\n@<[FirstProject]\\Build Administrators> \n```")]
        public void In_code(string markdownText)
        {
            Assert.DoesNotContain("<span class=\"mention-person\"", Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData(@"\@<Sebastian Raffel>")]
        [InlineData(@"\@<Gustav Mayer Gundron> \n")]
        [InlineData(@"\@<[FirstProject]\Build Administrators>")]
        [InlineData(@"\@<[FirstProject]\Build Administrators> \n")]
        public void Escaped_persons(string markdownText)
        {
            Assert.DoesNotContain("<span class=\"mention-person\"", Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
