using Microsoft.AspNet.Identity;

namespace CroquetAustralia.Library.Infrastructure
{
    public static class UserLoginInfoExtensions
    {
        public static string ToLogString(this UserLoginInfo login)
        {
            return $"{{LoginProvider: {login.LoginProvider}, ProviderKey: {login.ProviderKey}}}";
        }
    }
}