using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CroquetAustraliaWebsite.Library.Authentication.Domain;
using CroquetAustraliaWebsite.Library.Authentication.Domain.Roles;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Authentication.DAL
{
    public class InMemoryUserRepository : IUserRepository
    {
        private readonly List<DomainUser> _users;

        public InMemoryUserRepository()
        {
            // todo: remove hard coding.
            // I'm not concerned about storing these email addresses in public Git repository because the email addresses are already publicly known.
            _users = new List<DomainUser>()
            {
                new DomainUser(new Guid("BFCFD227-9019-40B1-8903-BEA017C08F32"), "Tim Murphy", "tim@26tp.com", new Role[] {new Developer(), new Editor()}, TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time")),
                new DomainUser(new Guid("CB16A14A-A15F-4854-9C71-6CB07F5EBFA2"), "Susan Linge", "croquet.australia@gmail.com", new Role[] {new Editor()}, TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time"))
            };

            // todo: validate unique ids are used.
        }

        public Task<IEnumerable<DomainUser>> FindAll()
        {
            return Task.FromResult(_users.AsEnumerable());
        }

        [return: AllowNull]
        public Task<DomainUser> FindByEmailAsync(string email)
        {
            return Task.FromResult(_users.SingleOrDefault(u => u.Email == email));
        }

        public Task<DomainUser> GetByIdAsync(Guid id)
        {
            var domainUser = _users.SingleOrDefault(u => u.Id == id);

            if (domainUser == null)
            {
                throw new Exception(string.Format("Cannot find user '{0}'.", id));
            }

            return Task.FromResult(domainUser);
        }
    }
}
