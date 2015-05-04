using System.ComponentModel.DataAnnotations;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.Infrastructure;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    public class AddNewsViewModel : AdminViewModel
    {
        private string _content;
        private string _title;

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