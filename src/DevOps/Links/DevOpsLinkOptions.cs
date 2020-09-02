using System;

namespace Markdig.Extensions.DevOps.Links
{
    public class DevOpsLinkOptions
    {
        public DevOpsLinkOptions()
        {
            OpenInNewWindow = true;
            Class = "mention-workitem";
        }

        public DevOpsLinkOptions(string url)
          : this()
        {
            Url = url;
        }

        public DevOpsLinkOptions(Uri uri)
          : this()
        {
            Url = uri.OriginalString;
        }

        public bool OpenInNewWindow { get; set; }
        public string Url { get; set; }
        public string Class { get; set; }
    }
}
