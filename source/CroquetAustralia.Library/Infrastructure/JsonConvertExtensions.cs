using System;
using Anotar.LibLog;
using Newtonsoft.Json;
using NullGuard;

namespace CroquetAustralia.Library.Infrastructure
{
    public static class JsonHelper
    {
        [return: AllowNull]
        public static string TrySerialize(object obj, Formatting formatting = Formatting.Indented)
        {
            try
            {
                return JsonConvert.SerializeObject(obj, formatting);
            }
            catch (Exception exception)
            {
                LogTo.WarnException("Could not serialize object.", exception);
                return null;
            }
        }
    }
}
