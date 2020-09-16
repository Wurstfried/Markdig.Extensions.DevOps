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
            Assert.Contains("<img ", Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData("width=\"20\"", "![Image alt](path/to.img \"Image title\" =20x)")]
        [InlineData("width=\"100\"", "![Image alt](path/to.img \"Image title\"  =100x20  )")]
        public void Image_width(string expected, string md)
        {
            Assert.Contains(expected, Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData("height=\"100\"", "![Image alt](path/to.img \"Image title\" =x100)")]
        [InlineData("height=\"20\"", "![Image alt](path/to.img \"Image title\"  =100x20  )")]
        public void Image_height(string expected, string md)
        {
            Assert.Contains(expected, Markdown.ToHtml(md, _pipeline));
        }
    }

    public class Does_not_parse
    {
        private readonly MarkdownPipeline _pipeline;

        public Does_not_parse() => _pipeline = new MarkdownPipelineBuilder().UseDevOpsImages().Build();

        [Theory]
        [InlineData("![Image alt](path/to.img \"Image title\" =X100)")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100X20  )")]
        public void Uppercase_x(string md)
        {
            Assert.DoesNotContain("<img", Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData("![Image alt](path/to.img \"Image title\" =100x50x20)")]
        public void Multiple_x(string md)
        {
            Assert.DoesNotContain("<img", Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData("![Image alt](path/to.img \"Image title\" = x100)")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100 x 20  )")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100 x20  )")]
        [InlineData("![Image alt](path/to.img \"Image title\"  = 100x  20  )")]
        public void Blanks_in_fix_size(string md)
        {
            Assert.DoesNotContain("<img", Markdown.ToHtml(md, _pipeline));
        }

        [Theory]
        [InlineData("![Image alt](path/to.img \"Image title\" = x100em)")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100ax20  )")]
        [InlineData("![Image alt](path/to.img \"Image title\"  =100x20Ä  )")]
        [InlineData("![Image alt](path/to.img \"Image title\"  = 100pxx20px  )")]
        public void Non_digits_in_fix_size(string md)
        {
            Assert.DoesNotContain("<img", Markdown.ToHtml(md, _pipeline));
        }
    }
}
