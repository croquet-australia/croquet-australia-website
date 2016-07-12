using System;
using System.Collections.Generic;
using CroquetAustralia.Library.Authentication.Domain.Roles;

namespace CroquetAustralia.Library.Authentication.Domain
{
    public class DomainUser
    {
        public DomainUser(Guid id, string name, string email, IEnumerable<Role> roles, TimeZoneInfo timeZoneInfo)
        {
            Id = id;
            Name = name;
            Email = email;
            Roles = roles;
            TimeZoneInfo = timeZoneInfo;
        }

        public Guid Id { get; private set; }
        public string Name { get; private set; }
        public string Email { get; private set; }
        public IEnumerable<Role> Roles { get; private set; }
        public TimeZoneInfo TimeZoneInfo { get; private set; }
    }
}