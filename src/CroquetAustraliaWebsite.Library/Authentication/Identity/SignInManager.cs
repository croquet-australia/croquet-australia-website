using System;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
{
    public class SignInManager : SignInManager<IdentityUser, Guid>
    {
        public SignInManager(UserManager<IdentityUser, Guid> userManager, IAuthenticationManager authenticationManager)
            : base(userManager, authenticationManager)
        {
        }
    }
}
