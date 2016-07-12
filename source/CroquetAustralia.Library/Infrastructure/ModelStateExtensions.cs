using System;
using System.Linq;
using System.Web.Mvc;

namespace CroquetAustralia.Library.Infrastructure
{
    public static class ModelStateExtensions
    {
        public static string ErrorsAsLoggingString(this ModelStateDictionary modelState)
        {
            var errorLines = from fieldErrors in modelState.Where(p => p.Value.Errors.Any())
                let fieldName = fieldErrors.Key
                let errors = fieldErrors.Value.Errors
                from error in errors
                select string.Format("{0} => message: '{1}', exception: '{2}'", fieldName, error.ErrorMessage, error.Exception);

            return string.Join(Environment.NewLine, errorLines);
        }
    }
}