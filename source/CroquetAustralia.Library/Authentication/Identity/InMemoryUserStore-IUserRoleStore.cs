using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;

namespace CroquetAustralia.Library.Authentication.Identity
{
    /// <summary>
    ///     Implements <see cref="IUserRoleStore{TUser,TKey}" />.
    /// </summary>
    public partial class InMemoryUserStore // : IUserRoleStore<IdentityUser, Guid>
    {
        public Task AddToRoleAsync(IdentityUser user, string roleName)
        {
            LogTo.Trace("AddToRoleAsync(user: {0}, roleName: {1})", user.ToLogString(), roleName);
            throw new NotImplementedException();
        }

        public Task<IList<string>> GetRolesAsync(IdentityUser user)
        {
            LogTo.Trace($"{nameof(GetRolesAsync)}(user: {user.ToLogString()})");

            if (user.Roles == null)
            {
                LogTo.Error($"Expected roles for user '{user.ToLogString()}' would not be null.");
            }

            var roles = user.Roles.ToArray();

            LogTo.Trace($"{nameof(GetRolesAsync)}(user: {user.ToLogString()}) -> roleCount: {roles.Length}");

            var roleNames = roles.Select(r => r.Name).ToList();

            LogTo.Trace($"{nameof(GetRolesAsync)}(user: {user.ToLogString()}) -> roles: {string.Join(", ", roleNames)}");

            return Task.FromResult<IList<string>>(roleNames);
        }

        public Task<bool> IsInRoleAsync(IdentityUser user, string roleName)
        {
            LogTo.Trace("IsInRoleAsync(user: {0}, roleName: {1})", user.ToLogString(), roleName);
            throw new NotImplementedException();
        }

        public Task RemoveFromRoleAsync(IdentityUser user, string roleName)
        {
            LogTo.Trace("RemoveFromRoleAsync(user: {0}, roleName: {1})", user.ToLogString(), roleName);
            throw new NotImplementedException();
        }
    }
}