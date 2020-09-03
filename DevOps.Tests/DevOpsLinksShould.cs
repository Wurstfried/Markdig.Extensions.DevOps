using Markdig;
using Markdig.Extensions.DevOps.Headings;
using Xunit;

namespace Markdig.Extensions.DevOps.Links.Tests
{
    public class DevOpsLinksShould
    {
        private readonly MarkdownPipeline _pipeline;

        public DevOpsLinksShould()
        {
            _pipeline = new MarkdownPipelineBuilder().UseDevOps(new DevOpsLinkOptions("https://dev.azure.com/org/prj"))
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
