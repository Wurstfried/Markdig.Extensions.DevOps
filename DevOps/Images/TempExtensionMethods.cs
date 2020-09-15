// Copyright (c) Sebastian Raffel. All rights reserved.
// This file is licensed under the BSD-Clause 2 license. 
// See the LICENSE file in the project root for more information.

using Markdig.Syntax.Inlines;

namespace Markdig.Extensions.DevOps.Images
{
    /// <summary>
    /// They are not meant to last...
    /// </summary>
    public static class TempExtensionMethods
    {
        public static T FirstParentOfType<T>(this Inline inline) where T : Inline
        {
            while (inline != null)
            {
                if (inline is T inlineOfT)
                {
                    return inlineOfT;
                }
                inline = inline.Parent;
            }
            return null;
        }
    }
}
