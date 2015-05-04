namespace CroquetAustraliaWebsite.Library.Settings
{
    public class SupportSettings : AppSettings
    {
        public SupportSettings()
            : base("Support")
        {
            Email = Get("Email");
            Name = Get("Name");
        }

        public string Email { get; private set; }
        public string Name { get; private set; }
    }
}
