using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using CroquetAustralia.Library.Infrastructure;
using Microsoft.AspNet.Identity;

namespace CroquetAustralia.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements the user store interface required my ASP.NET Identity system.
    /// </summary>
    /// <remarks>
    ///     Methods for each interface are in separate files. Shared fields, properties & methods are in this file.
    /// </remarks>
    public partial class InMemoryUserStore :
        // ReSharper disable once RedundantExtendsListEntry
        IUserStore<IdentityUser, Guid>,
        IUserLoginStore<IdentityUser, Guid>,
        IUserLockoutStore<IdentityUser, Guid>,
        IUserTwoFactorStore<IdentityUser, Guid>,
        IUserRoleStore<IdentityUser, Guid>
    {
        private static readonly ConcurrentDictionary<IdentityUser, List<UserLoginInfo>> Users = new ConcurrentDictionary<IdentityUser, List<UserLoginInfo>>();

        private void AddIdentityUser(IdentityUser user, List<UserLoginInfo> logins)
        {
            var added = Users.TryAdd(user, logins);

            if (!added)
            {
                throw new Exception($"Cannot add identity user '{user.Id}'.");
            }
        }

        private Task AddIdentityUserAsync(IdentityUser user)
        {
            return AddIdentityUserAsync(user, new List<UserLoginInfo>());
        }

        private Task AddIdentityUserAsync(IdentityUser user, List<UserLoginInfo> logins)
        {
            return Task.Run(() => AddIdentityUser(user, logins));
        }

        private IdentityUser FindIdentityUser(Func<KeyValuePair<IdentityUser, List<UserLoginInfo>>, bool> predicate)
        {
            var userLoginsPair = Users.SingleOrDefault(predicate);
            var foundUserLoginsPair = !userLoginsPair.Equals(default(KeyValuePair<IdentityUser, List<UserLoginInfo>>));
            var identityUser = foundUserLoginsPair ? userLoginsPair.Key : null;

            return identityUser;
        }

        private Task<IdentityUser> FindIdentityUserAsync(Func<KeyValuePair<IdentityUser, List<UserLoginInfo>>, bool> predicate)
        {
            return Task.FromResult(FindIdentityUser(predicate));
        }

        [Conditional("DEBUG")]
        // ReSharper disable once UnusedMember.Local
        private void LogAllUsersAndLogins()
        {
            LogTo.Debug("====== Start All Users & Logins =====");
            foreach (var pair in Users)
            {
                foreach (var login in pair.Value)
                {
                    LogTo.Debug("user: {0}, login: {1}", pair.Key.ToLogString(), login.ToLogString());
                }
            }
            LogTo.Debug("====== End All Users & Logins =====");
        }
    }
}