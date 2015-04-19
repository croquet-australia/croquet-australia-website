using System.Collections.Generic;
using System.Web;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public interface IViewModel
    {
        bool AddAngularSupport { get; }
        string ContainerClass { get; }
        IEnumerable<NavigationItem> NavigationItems { get; }
    }
}