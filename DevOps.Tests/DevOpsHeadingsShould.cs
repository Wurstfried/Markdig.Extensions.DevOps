using Markdig;
using Markdig.Extensions.DevOps.Links;
using Xunit;

namespace Markdig.Extensions.DevOps.Headings.Tests
{
    public class DevOpsHeadingsShould
    {
        private readonly MarkdownPipeline _pipeline;

        public DevOpsHeadingsShould()
        {
            _pipeline = new MarkdownPipelineBuilder().UseDevOps(new DevOpsLinkOptions("https://dev.azure.com/org/prj"))
                                                     .Build();
        }

        [Fact]
        public void ParseATXHeadings()
        {
            string sut = "# Title\nParagraph with *emphasis*\n## Subtitle with other words\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h1>", html);
            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void ParseWrappedATXHeadings()
        {
            string sut = "# Title #\nParagraph with *emphasis*\n## Subtitle with other words ##\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h1>", html);
            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void ParseHorridHeadings()
        {
            string sut = "#Title\nParagraph with *emphasis*\n##Subtitle with other words\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h1>", html);
            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void ParseWrappedHorridHeadings()
        {
            string sut = "#Title#\nParagraph with *emphasis*\n##Subtitle with other words##\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h1>", html);
            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void ParseMixedHeadings()
        {
            string sut = "#Title\nParagraph with *emphasis*\n## Subtitle with other words ##\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h1>", html);
            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void ParseHorridNumberHeadings()
        {
            string sut = "##123\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.Contains("<h2>", html);
        }

        [Fact]
        public void NotParseLinks()
        {
            string sut = "#123\n";

            string html = Markdown.ToHtml(sut, _pipeline);

            Assert.DoesNotContain("<h1>", html);
        }
    }
}
