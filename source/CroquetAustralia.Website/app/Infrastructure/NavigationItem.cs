using System.Linq;

namespace CroquetAustralia.Website.App.Infrastructure
{
    public class NavigationItem
    {
        public NavigationItem(string text, string url)
        {
            Text = text;
            Url = url.TrimStart('~');
            ChildNavigationItems = Enumerable.Empty<NavigationItem>().ToArray();
        }

        public NavigationItem(string text, params NavigationItem[] childNavigationItems)
        {
            Text = text;
            ChildNavigationItems = childNavigationItems;
        }

        public string Text { get; private set; }
        public string Url { get; private set; }
        public NavigationItem[] ChildNavigationItems { get; private set; }
    }
}