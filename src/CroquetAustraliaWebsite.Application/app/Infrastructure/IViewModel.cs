using System.Collections.Generic;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public interface IViewModel
    {
        bool AddAngularSupport { get; }
        string ContainerClass { get; }
        IEnumerable<NavigationItem> NavigationItems { get; }
    }
}