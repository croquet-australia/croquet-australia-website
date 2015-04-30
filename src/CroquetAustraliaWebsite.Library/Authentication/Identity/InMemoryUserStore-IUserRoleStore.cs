using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
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
            LogTo.Trace("GetRolesAsync(user: {0})", user.ToLogString());
            return Task.FromResult((IList<string>) user.Roles.Select(r => r.Name).ToList());
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