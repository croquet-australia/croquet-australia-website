using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System.Web;
using Anotar.LibLog;
using OpenMagic.Extensions;

namespace CroquetAustraliaWebsite.Library.Content
{
    public class PublishedBlogPostRepository : IPublishedBlogPostRepository
    {
        private readonly string _blogPostsDirectory;

        public PublishedBlogPostRepository(string blogPostsDirectory)
        {
            LogTo.Trace("blogPostsDirectory: {0}", blogPostsDirectory);

            _blogPostsDirectory = blogPostsDirectory;
        }

        public Task<IEnumerable<PublishedBlogPost>> GetAsync(IPagination pagination)
        {
            LogTo.Trace("GetAsync(pagination: {0})", pagination);

            return Task.FromResult(Get(pagination));
        }

        private IEnumerable<PublishedBlogPost> Get(IPagination pagination)
        {
            LogTo.Warn("todo: implement pagination");

            return from yearDirectory in Directory.EnumerateDirectories(_blogPostsDirectory).OrderByDescending(d => d) 
                   from monthDirectory in Directory.EnumerateDirectories(yearDirectory).OrderByDescending(d => d) 
                   from dayDirectory in Directory.EnumerateDirectories(monthDirectory).OrderByDescending(d => d) 
                   from blogPostFile in Directory.EnumerateFiles(dayDirectory, "*.cshtml").OrderByDescending(d => d) 
                   let content = File.ReadAllText(blogPostFile) 
                   let title = GetFirstHeading1(content) 
                   let published = GetPublished(yearDirectory, monthDirectory, dayDirectory) 
                   select new PublishedBlogPost(title, new HtmlString(RemoveFirstHeading1(content, title)), published);
        }

        private static DateTime GetPublished(string yearDirectory, string monthDirectory, string dayDirectory)
        {
            var year = GetDirectoryNumber(yearDirectory);
            var month = GetDirectoryNumber(monthDirectory);
            var day = GetDirectoryNumber(dayDirectory);

            return new DateTime(year, month, day);
        }

        private static int GetDirectoryNumber(string fullPath)
        {
            var name = fullPath.TextAfterLast("\\");
            var number = int.Parse(name);

            return number;
        }

        private static string GetFirstHeading1(string content)
        {
            // todo: proper implementation
            var h1 = content.TextAfter("<h1>");

            h1 = h1.TextBefore("</h1>");

            return h1;
        }

        private static string RemoveFirstHeading1(string content, string title)
        {
            return content.Replace("<h1>" + title + "</h1>", "");
        }
    }
}