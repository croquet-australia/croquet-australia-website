using Microsoft.AspNet.Identity;

namespace CroquetAustraliaWebsite.Library.Infrastructure
{
    public static class UserLoginInfoExtensions
    {
        public static string ToLogString(this UserLoginInfo login)
        {
            return string.Format("{{LoginProvider: {0}, ProviderKey: {1}}}", login.LoginProvider, login.ProviderKey);
        }
    }
}
