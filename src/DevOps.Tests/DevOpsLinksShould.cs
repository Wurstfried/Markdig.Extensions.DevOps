using Markdig.Extensions.DevOps.Headings;
using Microsoft.VisualStudio.TestPlatform.ObjectModel.Utilities;
using Xunit;

namespace Markdig.Extensions.DevOps.Links.Tests
{
    public class DevOpsLinksShould
    {
        private MarkdownPipeline _pipeline;

        public DevOpsLinksShould()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            _pipeline = MarkdownExtensions.Use<DevOpsLinksExtension>(pipelineBuilder)
                                          .Use<DevOpsHeadingsExtension>()
                                          .Build();
        }

        [Fact]
        public void ParseDevOpsLinks()
        {
            string sut = "#1234\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<a", html);
        }

        [Fact]
        public void ParseDevOpsLinksInParagraphs()
        {
            string sut = "A #1234 paragraph";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<a", html);
        }

        [Fact]
        public void ParseDevOpsLinksInHorridHeadings()
        {
            string sut = "#Horrid heading with #1234";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<a", html);
            Assert.Contains("<h1>", html);
        }

        [Fact]
        public void NotParseHorridHeadings()
        {
            string sut = "#12Horrid\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.DoesNotContain("<a", html);
            Assert.Contains("<h1>", html);
        }

        [Fact]
        public void NotParseInsideCodeInline()
        {
            string sut = "`#123`";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.DoesNotContain("<a", html);
            Assert.DoesNotContain("<h1>", html);
        }

        [Fact]
        public void NotParseInsideCodeBlock()
        {
            string sut = "```vb\n#123\n```";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.DoesNotContain("<a", html);
            Assert.DoesNotContain("<h1>", html);
        }
    }
}
