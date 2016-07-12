using System;
using ASP.NET.TestHelpers;
using CroquetAustralia.Website.Specifications.Helpers;

namespace CroquetAustralia.Website.Specifications.Pages.Infrastructure
{
    public abstract class Page : IPage
    {
        protected Page(string resource)
            : this(DependencyResolver.Resolve<WebApplication>().Uri, resource)
        {
        }

        private Page(Uri applicationUri, string relativeUri)
        {
            Url = new Uri(applicationUri, relativeUri).ToString();
        }

        public string Url { get; }
    }
}