namespace CroquetAustralia.Library.Settings
{
    public class WebApiSettings : AppSettings
    {
        public readonly string BaseUri;

        public WebApiSettings() : base("WebApi")
        {
            BaseUri = Get("BaseUri").TrimEnd('/');
        }
    }
}