using System;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustralia.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements <see cref="IUserStore{TUser,TKey}" />.
    /// </summary>
    public partial class InMemoryUserStore // : IUserStore<IdentityUser, Guid>
    {
        public Task CreateAsync(IdentityUser user)
        {
            LogTo.Trace("CreateAsync(user: {0})", user.ToLogString());
            return AddIdentityUserAsync(user);
        }

        public Task DeleteAsync(IdentityUser user)
        {
            LogTo.Trace("DeleteAsync(IdentityUser user)");
            throw new NotImplementedException();
        }

        public void Dispose()
        {
            LogTo.Trace("Dispose()");
        }

        [return: AllowNull]
        public async Task<IdentityUser> FindByIdAsync(Guid userId)
        {
            LogTo.Trace("FindByIdAsync(userId: {0})", userId);

            var identityUser = await FindIdentityUserAsync(pair => pair.Key.Id == userId);

            LogTo.Debug("FindByIdAsync(userId: {0}) => found: {1}", userId, identityUser != null);

            return identityUser;
        }

        [return: AllowNull]
        public async Task<IdentityUser> FindByNameAsync(string userName)
        {
            LogTo.Trace("FindByNameAsync(userName: {0})", userName);

            var identityUser = await FindIdentityUserAsync(pair => pair.Key.UserName == userName);

            LogTo.Debug("FindByNameAsync(userName: {0}) => found: {1}", userName, identityUser != null);

            return identityUser;
        }

        public Task UpdateAsync(IdentityUser user)
        {
            LogTo.Trace("UpdateAsync(user: {0})", user.ToLogString());
            // In memory store so nothing to update.
            return Task.FromResult(true);
        }
    }
}