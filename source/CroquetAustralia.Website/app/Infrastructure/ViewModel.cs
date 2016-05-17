using System.Collections.Generic;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public class ViewModel : IViewModel
    {
        public ViewModel()
            : this(true)
        {
        }

        public ViewModel(bool showSidebar)
            : this(PublicNavigationBar.GetNavigationItems(), showSidebar)
        {
        }

        public ViewModel(IEnumerable<NavigationItem> navigationItems)
            : this(navigationItems, true)
        {
        }

        public ViewModel(IEnumerable<NavigationItem> navigationItems, bool showSidebar)
            : this("container", navigationItems, showSidebar)
        {
        }

        public ViewModel(string containerClass, IEnumerable<NavigationItem> navigationItems, bool showSidebar)
        {
            ContainerClass = containerClass;
            NavigationItems = navigationItems;
            ShowSidebar = showSidebar;
        }

        public string ContainerClass { get; protected set; }
        public IEnumerable<NavigationItem> NavigationItems { get; set; }
        public bool ShowSidebar { get; }
    }
}