// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using System;

namespace Markdig.Extensions.DevOps.Links
{
    public class DevOpsLinkOptions
    {
        public DevOpsLinkOptions(){}

        public DevOpsLinkOptions(string url)
          : this() => Url = url;

        public DevOpsLinkOptions(Uri uri)
          : this() => Url = uri.OriginalString;

        public bool OpenInNewWindow { get; set; } = true;
        public string Url { get; set; }
        public string Class { get; set; } = "mention-workitem";
    }
}
