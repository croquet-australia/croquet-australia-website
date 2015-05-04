using System;
using System.Threading.Tasks;
using Anotar.LibLog;
using CroquetAustraliaWebsite.Library.Infrastructure;
using Microsoft.AspNet.Identity;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
{
    public class UserManager : UserManager<IdentityUser, Guid>
    {
        public UserManager(IUserStore<IdentityUser, Guid> store)
            : base(store)
        {
        }

        public async Task<bool> IsRegisteredAsync(UserLoginInfo login)
        {
            LogTo.Trace("IsRegisteredAsync(login: {0})", login.ToLogString());

            var user = await FindAsync(login);

            return user != null;
        }
    }
}
