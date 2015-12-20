using System;
using System.ComponentModel.DataAnnotations;
using System.IO;
using Anotar.LibLog;
using CroquetAustralia.Library.Infrastructure;
using CroquetAustralia.Library.IO;

namespace CroquetAustralia.Library.ComponentModel.DataAnnotations
{
    public class NewMarkdownPageAttribute : ValidationAttribute
    {
        private readonly IFileOperations _file;

        public NewMarkdownPageAttribute()
            : this(DependencyResolver.GetInstance<IFileOperations, FileOperations>())
        {
            LogTo.Trace("ctor()");
        }

        public NewMarkdownPageAttribute(IFileOperations fileOperations)
            : base("Page '{0}' already exists.")
        {
            _file = fileOperations;
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            LogTo.Trace("IsValid(value: {0}, validationContext: {1})", value, validationContext);

            // Required attribute will handle empty page names.
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Success;
            }

            var viewModel = validationContext.ObjectInstance as INewMarkdownPageViewModel;

            if (viewModel == null)
            {
                throw new Exception(string.Format("'{0}' must implement '{1}' for '{2}' to work with '{3}' property.", validationContext.ObjectType, typeof(INewMarkdownPageViewModel), typeof(NewMarkdownPageAttribute), validationContext.MemberName));
            }

            var fileName = FormatFileName(value.ToString());
            var directory = viewModel.FullDirectoryPath;
            var fullPath = Path.Combine(directory, fileName);

            return _file.Exists(fullPath)
                ? new ValidationResult(string.Format("Page '{0}' already exists.", value))
                : ValidationResult.Success;
        }

        protected virtual string FormatFileName(string pageName)
        {
            return pageName.EndsWith(".md", StringComparison.OrdinalIgnoreCase)
                ? pageName
                : pageName + ".md";
        }
    }
}
