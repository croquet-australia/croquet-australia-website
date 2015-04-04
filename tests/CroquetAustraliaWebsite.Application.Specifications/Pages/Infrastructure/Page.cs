using System;
using ASP.NET.TestHelpers;
using CroquetAustraliaWebsite.Application.Specifications.Helpers;

namespace CroquetAustraliaWebsite.Application.Specifications.Pages.Infrastructure
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

        public string Url { get; private set; }
    }
}