using Microsoft.AspNet.Identity;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
{
    public static class UserLoginInfoExtensions
    {
        public static bool IsEqual(this UserLoginInfo a, UserLoginInfo b)
        {
            return a.LoginProvider == b.LoginProvider && a.ProviderKey == b.ProviderKey;
        }
    }
}
