using System.Collections.Generic;

namespace CroquetAustraliaWebsite.Application.App.Infrastructure
{
    public class PublicNavigationBar
    {
        public static IEnumerable<NavigationItem> GetNavigationItems()
        {
            return new[]
            {
                new NavigationItem("Disciplines",
                    new NavigationItem("Golf Croquet",
                        new NavigationItem("Refereeing", "~/disciplines/golf-croquet/refereeing")))
            };
        }
    }
}