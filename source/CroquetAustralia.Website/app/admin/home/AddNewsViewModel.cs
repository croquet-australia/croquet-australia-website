using System.ComponentModel.DataAnnotations;
using CroquetAustralia.Library.Infrastructure;
using CroquetAustralia.Website.App.Infrastructure;

namespace CroquetAustralia.Website.App.admin.home
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