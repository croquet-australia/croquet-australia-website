using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using CroquetAustralia.Library.Infrastructure;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustralia.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements <see cref="IUserLoginStore{TUser,TKey}" />.
    /// </summary>
    public partial class InMemoryUserStore // : IUserLoginStore<IdentityUser, Guid>
    {
        public Task AddLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            LogTo.Trace("AddLoginAsync(user: {0}, login: {1})", user.ToLogString(), login.ToLogString());

            return Task.Run(() => AddLogin(user, login));
        }

        [return: AllowNull]
        public async Task<IdentityUser> FindAsync(UserLoginInfo login)
        {
            LogTo.Trace("FindAsync(login: {0})", login.ToLogString());

            var identityUser = await FindIdentityUserAsync(pair => { return pair.Value.SingleOrDefault(l => l.IsEqual(login)) != null; });
            var found = identityUser != null;

            LogTo.Debug("FindAsync(login: {0}) => found: {1}", login.ToLogString(), found);

            return identityUser;
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(IdentityUser user)
        {
            LogTo.Trace("GetLoginsAsync(IdentityUser user)");
            throw new NotImplementedException();
        }

        public Task RemoveLoginAsync(IdentityUser user, UserLoginInfo login)
        {
            LogTo.Trace("RemoveLoginAsync(IdentityUser user, UserLoginInfo login)");
            throw new NotImplementedException();
        }

        private void AddLogin(IdentityUser user, UserLoginInfo login)
        {
            List<UserLoginInfo> logins;

            var gotLogins = Users.TryGetValue(user, out logins);

            if (!gotLogins)
            {
                throw new Exception($"Cannot get logins for user '{user.Id}'.");
            }

            // ReSharper disable once PossibleNullReferenceException
            logins.Add(login);
        }
    }
}