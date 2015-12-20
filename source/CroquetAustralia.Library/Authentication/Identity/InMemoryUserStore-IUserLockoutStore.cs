using System;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;

namespace CroquetAustralia.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements <see cref="IUserLockoutStore{TUser,TKey}" />.
    /// </summary>
    public partial class InMemoryUserStore // : IUserLockoutStore<IdentityUser, Guid>
    {
        public Task<int> GetAccessFailedCountAsync(IdentityUser identityUser)
        {
            LogTo.Trace("GetAccessFailedCountAsync(IdentityUser identityUser)");
            throw new NotImplementedException();
        }

        public Task<bool> GetLockoutEnabledAsync(IdentityUser identityUser)
        {
            LogTo.Trace("GetLockoutEnabledAsync(identityUser: {0})", identityUser.ToLogString());
            return Task.FromResult(false);
        }

        public Task<DateTimeOffset> GetLockoutEndDateAsync(IdentityUser identityUser)
        {
            LogTo.Trace("GetLockoutEndDateAsync(IdentityUser identityUser)");
            throw new NotImplementedException();
        }

        public Task<int> IncrementAccessFailedCountAsync(IdentityUser identityUser)
        {
            LogTo.Trace("IncrementAccessFailedCountAsync(IdentityUser identityUser)");
            throw new NotImplementedException();
        }

        public Task ResetAccessFailedCountAsync(IdentityUser identityUser)
        {
            LogTo.Trace("IncrementAccessFailedCountAsync(IdentityUser identityUser)");
            throw new NotImplementedException();
        }

        public Task SetLockoutEnabledAsync(IdentityUser identityUser, bool enabled)
        {
            LogTo.Trace("SetLockoutEnabledAsync(IdentityUser identityUser, bool enabled)");
            throw new NotImplementedException();
        }

        public Task SetLockoutEndDateAsync(IdentityUser identityUser, DateTimeOffset lockoutEnd)
        {
            LogTo.Trace("SetLockoutEndDateAsync(IdentityUser identityUser, DateTimeOffset lockoutEnd)");
            throw new NotImplementedException();
        }
    }
}