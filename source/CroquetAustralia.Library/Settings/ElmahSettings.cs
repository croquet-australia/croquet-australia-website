namespace CroquetAustralia.Library.Settings
{
    public class ElmahSettings : AppSettings
    {
        public ElmahSettings()
            : base("Elmah")
        {
            ErrorLogType = Get("ErrorLogType");
            FilterErrorMessages = false;
        }

        public string ErrorLogType { get; private set; }
        public bool FilterErrorMessages { get; private set; }
    }
}