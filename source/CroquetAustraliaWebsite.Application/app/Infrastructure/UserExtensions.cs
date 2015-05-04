using Casper.Domain.Features.Authors;
using CroquetAustraliaWebsite.Library.Authentication.Domain;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public static class UserExtensions
    {
        public static Author ToAuthor(this DomainUser domainUser)
        {
            return new Author(domainUser.Name, domainUser.Email, domainUser.TimeZoneInfo);
        }
    }
}