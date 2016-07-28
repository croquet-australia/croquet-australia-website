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
            LogTo.Trace($"IsValid(value: {value}, validationContext: {validationContext})");

            // Required attribute will handle empty page names.
            if (string.IsNullOrWhiteSpace(value?.ToString()))
            {
                return ValidationResult.Success;
            }

            var viewModel = validationContext.ObjectInstance as INewMarkdownPageViewModel;

            if (viewModel == null)
            {
                throw new Exception($"'{validationContext.ObjectType}' must implement '{typeof(INewMarkdownPageViewModel)}' for '{typeof(NewMarkdownPageAttribute)}' to work with '{validationContext.MemberName}' property.");
            }

            var fileName = FormatFileName(value.ToString());
            var directory = viewModel.FullDirectoryPath;
            var fullPath = Path.Combine(directory, fileName);

            return _file.Exists(fullPath)
                ? new ValidationResult($"Page '{value}' already exists.")
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