using System.ComponentModel.DataAnnotations;
using System.IO;
using CroquetAustraliaWebsite.Application.App.Infrastructure;
using CroquetAustraliaWebsite.Library.ComponentModel.DataAnnotations;
using CroquetAustraliaWebsite.Library.Content;
using CroquetAustraliaWebsite.Library.Infrastructure;
using Microsoft.Practices.ServiceLocation;

namespace CroquetAustraliaWebsite.Application.App.admin.home
{
    public class NewPageViewModel : AdminViewModel, INewMarkdownPageViewModel
    {
        private string _directory;
        private string _gitRepositoryDirectory;
        private string _pageName;
        private string _content;
        private string _submitButton;

        public NewPageViewModel(string gitRepositoryDirectory, string directory)
        {
            Argument.CannotBeNullOrWhitespace("gitRepositoryDirectory", gitRepositoryDirectory);
            Argument.CannotBeNull("directory", directory);

            _gitRepositoryDirectory = gitRepositoryDirectory;

            Directory = directory;
        }

        /// <summary>
        ///     Gets the Git repository directory.
        /// </summary>
        /// <remarks>
        ///     todo
        /// </remarks>
        protected string GitRepositoryDirectory
        {
            get { return _gitRepositoryDirectory ?? (_gitRepositoryDirectory = ServiceLocator.Current.GetInstance<IGitContentRepository>().Directory); }
        }

        public string Directory
        {
            get { return _directory ?? ""; }
            set { _directory = value.AsTrimmedString(); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Page name is required.")]
        [NewMarkdownPage]
        public string PageName
        {
            get { return _pageName ?? ""; }
            set { _pageName = value.AsTrimmedString(); }
        }

        [Required(AllowEmptyStrings = false, ErrorMessage = "Content is required.")]
        public string Content
        {
            get { return _content ?? ""; }
            set { _content = value.AsTrimmedString(); }
        }

        public string SubmitButton
        {
            get { return _submitButton ?? ""; }
            set { _submitButton = value.AsTrimmedString(); }
        }

        public string FullDirectoryPath
        {
            get { return Path.Combine(GitRepositoryDirectory, Directory); }
        }
    }
}