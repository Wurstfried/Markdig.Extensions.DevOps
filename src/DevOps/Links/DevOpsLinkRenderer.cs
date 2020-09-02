using Markdig.Helpers;
using Markdig.Renderers;
using Markdig.Renderers.Html;

namespace Markdig.Extensions.DevOps.Links
{
    public class DevOpsLinkRenderer : HtmlObjectRenderer<DevOpsLink>
    {
        private readonly DevOpsLinkOptions _options;

        public DevOpsLinkRenderer(DevOpsLinkOptions options) => _options = options;

        protected override void Write(HtmlRenderer renderer, DevOpsLink link)
        {
            StringSlice issueNumber = link.IssueNumber;

            if (renderer.EnableHtmlForInline)
            {
                renderer.Write("<a href=\"").Write(_options.Url).Write("/_workitems/edit/").Write(issueNumber).Write("\"");
                renderer.Write(" class=\"").Write(_options.Class).Write("\"");

                if (_options.OpenInNewWindow)
                    renderer.Write(" target=\"blank\" rel=\"noopener noreferrer\"");

                renderer.Write('>').Write(issueNumber).Write(" </a>");
            }
            else
            {
                renderer.Write('#').Write(link.IssueNumber);
            }
        }
    }
}
