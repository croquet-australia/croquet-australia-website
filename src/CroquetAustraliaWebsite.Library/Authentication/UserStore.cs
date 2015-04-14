using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Anotar.LibLog;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Authentication
{
    public class UserStore<TUser> : IUserStore<TUser> where TUser : class, IUser<string>
    {
        private readonly static Dictionary<string, TUser> Users = new Dictionary<string, TUser>();

        public async Task CreateAsync(TUser user)
        {
            LogTo.Trace("CreateAsync(user: {0})", user);

            if (await FindByIdAsync(user.Id) != null)
            {
                var message = string.Format("Cannot add duplicate users '{0}'.", user.Id);
                LogTo.Error(message);
                throw new Exception(message);
            }

            Users.Add(user.Id, user);

            LogTo.Debug("Created user: {{Id: {0}, UserName: {1}}}", user.Id, user.UserName);
        }

        public Task DeleteAsync(TUser user)
        {
            LogTo.Warn("NotImplemented");
            throw new NotImplementedException();
        }

        public void Dispose()
        {
        }

        [return: AllowNull]
        public Task<TUser> FindByIdAsync(string userId)
        {
            LogTo.Trace("FindByIdAsync(userId: {0})", userId);

            TUser user;

            Users.TryGetValue(userId, out user);

            LogTo.Debug("FindByIdAsync(userId: {0}) => {1}", userId, user == null ? "not found" : "found");

            return Task.FromResult(user);
        }

        public Task<TUser> FindByNameAsync(string userName)
        {
            LogTo.Trace("FindByNameAsync(userName: {0})", userName);

            var user = Users.Values.SingleOrDefault(u => u.UserName == userName);


            LogTo.Debug("FindByNameAsync(userName: {0}) => {1}", userName, user == null ? "not found" : "found");

            return Task.FromResult(user);
        }

        public Task UpdateAsync(TUser user)
        {
            LogTo.Trace("UpdateAsync(user: {0})", user);

            Users[user.Id] = user;
            return Task.FromResult(0);
        }
    }
}