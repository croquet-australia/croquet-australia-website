using System;
using System.Collections.Generic;
using CroquetAustraliaWebsite.Library.Authentication.Domain;
using CroquetAustraliaWebsite.Library.Authentication.Domain.Roles;
using CroquetAustraliaWebsite.Library.Infrastructure;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Authentication.Identity
{
    public class IdentityUser : IUser<Guid>
    {
        private readonly DomainUser _domainUser;

        public IdentityUser(DomainUser domainUser)
        {
            _domainUser = domainUser;
        }

        public Guid Id
        {
            get { return _domainUser.Id; }
        }

        public IEnumerable<Role> Roles
        {
            get { return _domainUser.Roles; }
        }

        public string UserName
        {
            get { return Base64.Encode(_domainUser.Email); }
            set { throw new NotSupportedException(); }
        }

        public override bool Equals([AllowNull] object obj)
        {
            return Equals((IdentityUser) obj);
        }

        public override int GetHashCode()
        {
            return Id.GetHashCode();
        }

        internal string ToLogString()
        {
            return string.Format("{{Id: {0}, UserName: {1}}}", Id, UserName);
        }

        private bool Equals([AllowNull] IdentityUser identityUser)
        {
            return identityUser != null && identityUser.Id == Id;
        }
    }
}