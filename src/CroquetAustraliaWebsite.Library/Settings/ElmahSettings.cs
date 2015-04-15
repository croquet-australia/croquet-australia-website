using System;
using System.Web;

namespace CroquetAustraliaWebsite.Library.Settings
{
    public class ElmahSettings : AppSettings
    {
        public ElmahSettings()
            : base("Elmah")
        {
            ErrorLogType = Get("ErrorLogType");
            FilterErrorMessages = ErrorLogType.Equals("Elmah.Io", StringComparison.OrdinalIgnoreCase);
            LogId = Get("LogId");
        }

        public string ErrorLogType { get; private set; }
        public string LogId { get; private set; }
        public bool FilterErrorMessages { get; private set; }
    }
}
