using System.Collections.Generic;
using System.Linq;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public class AdminViewModel : ViewModel
    {
        public AdminViewModel()
            : this(GetNavigationItems())
        {
        }

        public AdminViewModel(IEnumerable<NavigationItem> navigationItems)
            : base("container-fluid", navigationItems)
        {
        }

        private static IEnumerable<NavigationItem> GetNavigationItems()
        {
            return Enumerable.Empty<NavigationItem>();
        }
    }
}