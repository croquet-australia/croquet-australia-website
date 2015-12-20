using System.Collections.Generic;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public class ViewModel : IViewModel
    {
        public ViewModel()
            : this(PublicNavigationBar.GetNavigationItems())
        {
        }

        public ViewModel(IEnumerable<NavigationItem> navigationItems)
            : this("container", navigationItems)
        {
        }

        public ViewModel(string containerClass, IEnumerable<NavigationItem> navigationItems)
        {
            ContainerClass = containerClass;
            NavigationItems = navigationItems;
        }

        public string ContainerClass { get; protected set; }
        public IEnumerable<NavigationItem> NavigationItems { get; set; }
    }
}