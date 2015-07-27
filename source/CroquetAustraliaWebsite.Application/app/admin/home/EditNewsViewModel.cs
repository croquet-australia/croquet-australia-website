using System.ComponentModel.DataAnnotations;
using Casper.Domain.Features.BlogPosts;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Infrastructure;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    public class EditNewsViewModel : AdminViewModel
    {
        private string _content;
        private string _title;

        public EditNewsViewModel()
        {
            // POST action requires empty constructor.
        }
        public EditNewsViewModel(BlogPost blogPost)
        {
            Title = blogPost.Title;
            Content = blogPost.Content;
            RelativeUri = blogPost.RelativeUri;
        }

        public string RelativeUri { get; set; }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Title is required.")]
        public string Title
        {
            get { return _title ?? ""; }
            set { _title = value.AsTrimmedString(); }
        }
        
        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is required.")]
        public string Content
        {
            get { return _content ?? ""; }
            set { _content = value.AsTrimmedString(); }
        }
    }
}