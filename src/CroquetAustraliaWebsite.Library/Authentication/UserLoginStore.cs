using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;
using Microsoft.Owin.Security.Provider;

namespace CroquetAustraliaWebsite.Library.Authentication
{
    /// <summary>
    ///     <para>
    ///         Implements <see cref="IUserStore{TUser}" /> - Interface that exposes basic user management apis
    ///     </para>
    ///     <para>
    ///         <see cref="IUserLoginStore{TUser}" /> - Interface that maps users to login providers, i.e. Google, Facebook,
    ///         Twitter, Microsoft
    ///     </para>
    /// </summary>
    public class UserLoginStore<TUser> : UserStore<TUser>, IUserLoginStore<TUser> where TUser : class, IUser<string>
    {
        private static readonly ConcurrentDictionary<TUser, IList<UserLoginInfo>> Users = new ConcurrentDictionary<TUser, IList<UserLoginInfo>>();

        public Task AddLoginAsync(TUser user, UserLoginInfo login)
        {
            LogTo.Trace("AddLoginAsync(user: {0}, login: {{LoginProvider: {1}, ProviderKey: {2}}}", user, login.LoginProvider, login.ProviderKey);

            var logins = Users.GetOrAdd(user, new List<UserLoginInfo>());

            logins.Add(login);

            return Task.FromResult(0);
        }

        public Task RemoveLoginAsync(TUser user, UserLoginInfo login)
        {
            LogTo.Trace("RemoveLoginAsync(user: {0}, login: {{LoginProvider: {1}, ProviderKey: {2}}}", user, login.LoginProvider, login.ProviderKey);
            throw new NotImplementedException("todo: RemoveLoginAsync");
        }

        public Task<IList<UserLoginInfo>> GetLoginsAsync(TUser user)
        {
            LogTo.Trace("GetLogins(user: {0})", user);
            return Task.FromResult(Users[user]);
        }

        /// <summary>
        ///     Returns the user associated with this login.
        /// </summary>
        /// <remarks>
        ///     <see cref="IUserLoginStore{TUser}.FindAsync(UserLoginInfo)" />
        /// </remarks>
        public Task<TUser> FindAsync(UserLoginInfo login)
        {
            LogTo.Trace("FindAsync(login: {{LoginProvider: {0}, ProviderKey: {1}}})", login.LoginProvider, login.ProviderKey);

            var user = (
                from u in Users
                where u.Value.Any(l => l.LoginProvider == login.LoginProvider && l.ProviderKey == login.ProviderKey)
                select u.Key
                ).SingleOrDefault();

            LogTo.Debug("FindAsync(login: {{LoginProvider: {0}, ProviderKey: {1}}}) => {2}", login.LoginProvider, login.ProviderKey, user == null ? "not found" : "found");

            return Task.FromResult(user);
        }
    }
}