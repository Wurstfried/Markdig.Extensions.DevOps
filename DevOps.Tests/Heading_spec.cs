// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig;
using Markdig.Extensions.DevOps;
using Xunit;

namespace Heading_spec
{
    public class Parses
    {
        private readonly MarkdownPipeline _pipeline;

        public Parses() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsHeadings().Build();

        [Theory]
        [InlineData("# Title"                             , "<h1>")]
        [InlineData("## Title and blanks"                 , "<h2>")]
        [InlineData(" # Title "                           , "<h1>")]
        [InlineData("# Title\nBla *bla*\n## Another title", "<h1>")]
        public void ATX_headings(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("# Title #"                              , "<h1>")]
        [InlineData("## Title and blanks ####"               , "<h2>")]
        [InlineData(" # Title #"                             , "<h1>")]
        [InlineData("# Title ##\nBla *bla*\n## Another title", "<h1>")]
        public void Wrapped_ATX_headings(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#Title"                             , "<h1>")]
        [InlineData("##Title and blanks"                 , "<h2>")]
        [InlineData(" #Title"                            , "<h1>")]
        [InlineData("# Title\nBla *bla*\n##Another title", "<h2>")]
        public void Horrid_headings(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#Title #"                             , "<h1>")]
        [InlineData("##Title and blanks ####"              , "<h2>")]
        [InlineData(" #Title #"                            , "<h1>")]
        [InlineData("#Title ##\nBla *bla*\n##Another title", "<h1>")]
        public void Wrapped_horrid_headings(string markdownText, string expected)
        {
            Assert.Contains(expected, Markdown.ToHtml(markdownText, _pipeline));
        }

        [Theory]
        [InlineData("#Title #\n## Title"                    , "<h1>", "<h2>")]
        [InlineData("##Title and blanks ####\n\n# Title"    , "<h1>", "<h2>")]
        [InlineData(" #Title\n## Title2"                    , "<h1>", "<h2>")]
        [InlineData("#Title ##\nBla *bla*\n## Another title", "<h1>", "<h2>")]
        public void Mixed_headings(string markdownText, string expected, string expected2)
        {
            string html = Markdown.ToHtml(markdownText, _pipeline);
            Assert.Contains(expected, html);
            Assert.Contains(expected2, html);
        }

        [Fact]
        public void Horrid_number_headings()
        {
            string sut = "##123\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h2>", html);
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOps(new DevOpsLinkOptions("https://dev.azure.com/org/prj")).Build();

        [Theory]
        [InlineData("#9"    , "<h1>")]
        [InlineData("#123"  , "<h1>")]
        [InlineData("#12345", "<h1>")]
        [InlineData("#123\n", "<h1>")]
        public void WorkItems(string markdownText, string expected)
        {
            Assert.DoesNotContain(expected, Markdown.ToHtml(markdownText, _pipeline));
        }
    }
}
