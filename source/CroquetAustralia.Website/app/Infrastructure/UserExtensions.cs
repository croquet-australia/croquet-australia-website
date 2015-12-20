using Casper.Domain.Features.Authors;
using CroquetAustralia.Library.Authentication.Domain;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public static class UserExtensions
    {
        public static Author ToAuthor(this DomainUser domainUser)
        {
            return new Author(domainUser.Name, domainUser.Email, domainUser.TimeZoneInfo);
        }
    }
}