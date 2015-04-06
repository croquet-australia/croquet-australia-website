using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Anotar.LibLog;
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
            UserName = id.Replace("-", "");
            Name = name;
            Email = email;

            IsEditor = new[] { "tim@26tp.com", "croquet.australia@gmail.com" }.Contains(email);
        }

        public string Name { get; set; }
        public string Email { get; set; }
        public string Id { get; private set; }
        public string UserName { get; set; }
        private bool IsEditor { get; set; }

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

            if (IsEditor)
            {
                userIdentity.AddClaim(new Claim(ClaimTypes.Role, "Editor"));
            }

            return userIdentity;
        }

        public override int GetHashCode()
        {
            return (Id != null ? Id.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return string.Format("{0} <{1}>", Name, Email);
        }
    }
}