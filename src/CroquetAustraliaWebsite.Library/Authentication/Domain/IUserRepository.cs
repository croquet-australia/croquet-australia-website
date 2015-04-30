using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CroquetAustraliaWebsite.Library.Authentication.Domain
{
    public interface IUserRepository
    {
        Task<IEnumerable<DomainUser>> FindAll();
        Task<DomainUser> FindByEmailAsync(string email);
        Task<DomainUser> GetByIdAsync(Guid id);
    }
}