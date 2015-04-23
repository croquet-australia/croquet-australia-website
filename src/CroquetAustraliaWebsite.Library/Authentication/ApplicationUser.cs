using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Anotar.LibLog;
using Casper.Core;
using Casper.Domain.Features.Authors;
using CroquetAustraliaWebsite.Library.Infrastructure;
using Microsoft.AspNet.Identity;
using NullGuard;

namespace CroquetAustraliaWebsite.Library.Authentication
{
    public class ApplicationUser : IUser<string>
    {
        public ApplicationUser(string name, string email)
            : this(Guid.NewGuid().ToString(), name, email)
        {
        }

        private ApplicationUser(string id, string name, string email)
        {
            LogTo.Trace("ctor");

            Id = id;
            UserName = Base64.Encode(email);
            Name = name;
            Email = email;

            // todo: remove hard coding.
            Roles = new List<string>();

            if (new[] {"tim@26tp.com", "croquet.australia@gmail.com"}.Contains(email))
            {
                Roles.Add("Editor");
            }

            if (email.Equals("tim@26tp.com"))
            {
                Roles.Add("Developer");
            }

            TimeZoneInfo = TimeZoneInfo.FindSystemTimeZoneById("AUS Eastern Standard Time");
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; private set; }
        public string UserName { get; set; }
        public TimeZoneInfo TimeZoneInfo { get; set; }

        private IList<string> Roles { get; set; }

        public override bool Equals([AllowNull] object obj)
        {
            return Equals(obj as ApplicationUser);
        }

        protected bool Equals([AllowNull]ApplicationUser other)
        {
            return other != null && string.Equals(Id, other.Id);
        }

        public async Task<ClaimsIdentity> GenerateUserIdentityAsync(UserManager<ApplicationUser> manager)
        {
            LogTo.Trace("GenerateUserIdentityAsync(manager: {0})", manager);

            var userIdentity = await manager.CreateIdentityAsync(this, DefaultAuthenticationTypes.ApplicationCookie);

            if (!userIdentity.IsAuthenticated)
            {
                throw new Exception("Expected user to be authenticated.");
            }

            foreach (var role in Roles)
            {
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, role));
            }

            return userIdentity;
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public Author ToAuthor()
        {
            return new Author(Name, Email, TimeZoneInfo);
        }

        public override string ToString()
        {
            return string.Format("{0} <{1}>", Name, Email);
        }
    }
}