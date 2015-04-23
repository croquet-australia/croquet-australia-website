﻿using System.Linq;
using Humanizer;
using OpenMagic.Extensions;

namespace CroquetAustraliaWebsite.Library.Infrastructure
{
    // ReSharper disable once InconsistentNaming
    public static class IMarkdownStringExtensions
    {
        public static string GetPageTitle(this IMarkdownString markdownString, string relativeUri)
        {
            var text = markdownString.ToString();
            var lines = text.ToLines(true);
            var heading1Line = lines.FirstOrDefault(line => line.StartsWith("#") && !line.StartsWith("##"));
            var pageTitle = heading1Line == null ? relativeUri.Humanize() : heading1Line.TextAfter("#").Trim();

            return pageTitle;
        }
    }
}
