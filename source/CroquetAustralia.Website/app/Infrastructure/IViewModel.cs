using System.Collections.Generic;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public interface IViewModel
    {
        string ContainerClass { get; }
        IEnumerable<NavigationItem> NavigationItems { get; }
    }
}