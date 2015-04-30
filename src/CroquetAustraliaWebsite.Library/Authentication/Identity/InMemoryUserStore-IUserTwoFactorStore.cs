using System;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements <see cref="IUserTwoFactorStore{TUser,TKey}" />.
    /// </summary>
    public partial class InMemoryUserStore // : IUserTwoFactorStore<IdentityUser, Guid>
    {
        public Task SetTwoFactorEnabledAsync(IdentityUser user, bool enabled)
        {
            LogTo.Trace("SetTwoFactorEnabledAsync(user: {0}, enabled: {1})", user.ToLogString(), enabled);
            throw new NotImplementedException();
        }

        public Task<bool> GetTwoFactorEnabledAsync(IdentityUser user)
        {
            LogTo.Trace("GetTwoFactorEnabledAsync(user: {0})", user.ToLogString());
            return Task.FromResult(false);
        }
    }
}