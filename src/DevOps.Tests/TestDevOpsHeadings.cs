using Xunit;

namespace Markdig.Extensions.DevOps.Headings.Tests
{
    public class TestDevOpsHeadings
    {
        private MarkdownPipeline _pipeline;

        private const string _mdHeaderHorrid        = "#Title\nParagraph with *emphasis*\n##Subtitle with other words\n";
        private const string _mdHeaderHorridWrapped = "#Title#\nParagraph with *emphasis*\n##Subtitle with other words##\n";

        private const string _mdHeaderGood          = "# Title\nParagraph with *emphasis*\n## Subtitle with other words\n";
        private const string _mdHeaderGoodWrapped   = "# Title #\nParagraph with *emphasis*\n## Subtitle with other words ##\n";
        private const string _mdHeaderMixWrapped    = "#Title\nParagraph with *emphasis*\n## Subtitle with other words ##\n";

        public TestDevOpsHeadings()
        {
            var pipelineBuilder = new MarkdownPipelineBuilder();
            pipelineBuilder     = MarkdownExtensions.Use<DevOpsIDsExtension>(pipelineBuilder);
            _pipeline           = pipelineBuilder.Build();
        }

        [Fact]
        public void ParsesGoodHeadings()
        {
            string html1 = Markdown.ToHtml(_mdHeaderGood       , _pipeline);
            string html2 = Markdown.ToHtml(_mdHeaderGoodWrapped, _pipeline);

            Assert.Contains("<h1>", html1);
            Assert.Contains("<h1>", html2);
            Assert.Contains("<h2>", html1);
            Assert.Contains("<h2>", html2);
        }

        [Fact]
        public void ParsesBadHeadings()
        {
            string html1 = Markdown.ToHtml(_mdHeaderHorrid       , _pipeline);
            string html2 = Markdown.ToHtml(_mdHeaderHorridWrapped, _pipeline);

            Assert.Contains("<h1>", html1);
            Assert.Contains("<h1>", html2);
            Assert.Contains("<h2>", html1);
            Assert.Contains("<h2>", html2);
        }

        [Fact]
        public void ParsesMixedHeadings()
        {
            string html1 = Markdown.ToHtml(_mdHeaderMixWrapped, _pipeline);
            Assert.Contains("<h1>", html1);
            Assert.Contains("<h2>", html1);
        }
    }
}
