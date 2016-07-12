using System;
using System.Collections.Generic;
using CroquetAustralia.Library.Authentication.Domain;
using CroquetAustralia.Library.Authentication.Domain.Roles;
using CroquetAustralia.Library.Infrastructure;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustralia.Library.Authentication.Identity
{
    public class IdentityUser : IUser<Guid>
    {
        private readonly DomainUser _domainUser;

        public IdentityUser(DomainUser domainUser)
        {
            _domainUser = domainUser;
        }

        public IEnumerable<Role> Roles
        {
            get { return _domainUser.Roles; }
        }

        public Guid Id
        {
            get { return _domainUser.Id; }
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