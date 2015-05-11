using System.Collections.Generic;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public interface IViewModel
    {
        string ContainerClass { get; }
        IEnumerable<NavigationItem> NavigationItems { get; }
    }
}