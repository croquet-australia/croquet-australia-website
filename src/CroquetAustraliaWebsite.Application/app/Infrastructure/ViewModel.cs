using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class ViewModel : IViewModel
    {
        private const string DownArrowHtml = ""; // @"<span class=""caret""></span>";

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

        public bool AddAngularSupport { get; protected set; }
        public string ContainerClass { get; protected set; }
        public IEnumerable<NavigationItem> NavigationItems { get; set; }
   }
}